using AutoMapper;
using CTS_BE.BAL.Interfaces.Pension;
using CTS_BE.DAL;
using CTS_BE.DAL.Entities.Pension;
using CTS_BE.DAL.Interfaces.Pension;
using CTS_BE.DTOs;
using CTS_BE.Helper;
using CTS_BE.Helper.Authentication;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.BAL.Services.Pension
{
    public class EPpoReceiptService(
        IEPpoReceiptRepository ePpoReceiptRepository,
        IMapper mapper,
        IClaimService claimService
    ) : BaseService(claimService), IEPpoReceiptService
    {
        private readonly IEPpoReceiptRepository _ePpoReceiptRepository = ePpoReceiptRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<T> CreateEPpoReceipt<T>(
            EPpoReceiptEntryDTO ePpoReceiptEntryDTO,
            string treasuryCode,
            short financialYear
        )
        {
            EppoReceipt eppoReceipt = _mapper.Map<EppoReceipt>(ePpoReceiptEntryDTO);
            try
            {
                eppoReceipt.FinancialYear = financialYear;
                eppoReceipt.Withdrawn = false;
                SetCreatedBy(eppoReceipt);

                // Prepare file paths and MIME types
                PrepareEppoFiles(eppoReceipt, treasuryCode, financialYear);

                // Create the PpoReceipt entity
                var dateOfCommencement = eppoReceipt.DateOfRetirement.AddDays(1);
                var ppoReceipt = new PpoReceipt
                {
                    PpoNo = eppoReceipt.PpoNo,
                    ReceiptType = "EPPO",
                    TreasuryReceiptNo = eppoReceipt.PensionApplnNo,
                    PsaCode = 'D',
                    PpoType = eppoReceipt.PpoTypeCode,
                    PensionerName = eppoReceipt.PensionerName,
                    MobileNumber = eppoReceipt.MobileNumber,
                    DateOfCommencement = dateOfCommencement,
                    ReceiptDate = dateOfCommencement.AddDays(1),
                    TreasuryCode = treasuryCode,
                    FinancialYear = financialYear,
                    PpoStatus = "EPPO Received",
                    ActiveFlag = true,
                };
                eppoReceipt.PpoReceipts = new List<PpoReceipt> { ppoReceipt };
                ppoReceipt.EppoReceipt = eppoReceipt;

                // Check if eppoReceipt already exists
                EppoReceipt? eppoReceiptExists =
                    await _ePpoReceiptRepository.GetEPpoReceiptByPensionApplnNo(
                        eppoReceipt.PensionApplnNo,
                        treasuryCode,
                        financialYear
                    );

                if (eppoReceiptExists != null)
                {
                    var response = _mapper.Map<T>(eppoReceipt);
                    response.FillDataSource(
                        eppoReceiptExists,
                        "eppoReceipt already exists for Pension Application No: "
                            + eppoReceipt.PensionApplnNo
                    );
                    return response;
                }

                T savedResponse = await _ePpoReceiptRepository.SaveEPpoReceipt<T>(
                    eppoReceipt,
                    ppoReceipt,
                    entity => _mapper.Map<T>(entity)
                );
                return savedResponse;
            }
            catch (Exception ex)
            {
                var response = _mapper.Map<T>(eppoReceipt);
                response.FillDataSource(
                    eppoReceipt,
                    $"ServiceException: {ex.InnerException?.Message ?? ex.Message}"
                );
                return response;
            }
        }

        private static void PrepareEppoFiles(
            EppoReceipt eppoReceipt,
            string treasuryCode,
            short financialYear
        )
        {
            FileExtensionContentTypeProvider provider = new();
            string basePath = $"{treasuryCode}/{financialYear}/";

            if (eppoReceipt.EppoFile != null)
            {
                eppoReceipt.EppoFile.FilePath = basePath;
                provider.TryGetContentType(eppoReceipt.EppoFile.FileName, out string? mimeType);
                eppoReceipt.EppoFile.FileMimeType = mimeType ?? "application/octet-stream";
            }

            if (eppoReceipt.PhotoFile != null)
            {
                eppoReceipt.PhotoFile.FilePath = basePath;
                provider.TryGetContentType(eppoReceipt.PhotoFile.FileName, out string? mimeType);
                eppoReceipt.PhotoFile.FileMimeType = mimeType ?? "application/octet-stream";
            }

            if (eppoReceipt.SignatureFile != null)
            {
                eppoReceipt.SignatureFile.FilePath = basePath;
                provider.TryGetContentType(
                    eppoReceipt.SignatureFile.FileName,
                    out string? mimeType
                );
                eppoReceipt.SignatureFile.FileMimeType = mimeType ?? "application/octet-stream";
            }
        }

        public async Task<T> CreateEPpoReceiptRevision<T>(
            EPpoReceiptRevisionEntryDTO ePpoReceiptRevisionEntryDTO,
            string treasuryCode,
            short financialYear
        )
        {
            EppoRevision eppoRevision = _mapper.Map<EppoRevision>(ePpoReceiptRevisionEntryDTO);
            T response = _mapper.Map<T>(eppoRevision);

            try
            {
                eppoRevision.TreasuryCode = treasuryCode;
                eppoRevision.FinancialYear = financialYear;
                SetCreatedBy(eppoRevision);

                response = await _ePpoReceiptRepository.SaveRevisedEPpoReceipt<T>(
                    eppoRevision,
                    treasuryCode,
                    financialYear,
                    entity => _mapper.Map<T>(entity)
                );
            }
            catch (Exception ex)
            {
                response.FillDataSource(
                    eppoRevision,
                    $"ServiceException: {ex.InnerException?.Message ?? ex.Message}"
                );
            }
            return response;
        }

        public async Task<T> GetEPpoReceiptById<T>(
            long receiptId,
            string treasuryCode,
            short financialYear
        )
        {
            T? response = default(T);
            try
            {
                EppoReceipt? eppoReceipt = await _ePpoReceiptRepository.GetEPpoReceiptById(
                    receiptId,
                    treasuryCode,
                    financialYear
                );

                if (eppoReceipt is null)
                {
                    response = _mapper.Map<T>(new EPpoReceiptDetailDTO());
                    response.FillDataSource(
                        eppoReceipt,
                        $"No record found for Receipt ID: {receiptId}"
                    );
                    return response;
                }

                response = _mapper.Map<T>(eppoReceipt);
                return response;
            }
            catch (Exception ex)
            {
                response = _mapper.Map<T>(new EPpoReceiptDetailDTO());
                response.FillDataSource(
                    new EppoReceipt(),
                    $"ServiceException: {ex.InnerException?.Message ?? ex.Message}"
                );
                return response;
            }
        }

        public async Task<T> GetEPpoReceiptByPensionApplnNo<T>(
            string pensionApplnNo,
            string treasuryCode,
            short financialYear
        )
        {
            T? response = _mapper.Map<T>(new EppoReceipt() { PensionApplnNo = pensionApplnNo });
            try
            {
                EppoReceipt? eppoReceipt =
                    await _ePpoReceiptRepository.GetEPpoReceiptByPensionApplnNo(
                        pensionApplnNo,
                        treasuryCode,
                        financialYear
                    );

                if (eppoReceipt is null)
                {
                    response.FillDataSource(
                        eppoReceipt,
                        "No record found for Pension Application No: " + pensionApplnNo
                    );
                    return response;
                }

                response = _mapper.Map<T>(eppoReceipt);
                return response;
            }
            catch (Exception ex)
            {
                response.FillDataSource(
                    new EppoReceipt(),
                    $"ServiceException: {ex.InnerException?.Message ?? ex.Message}"
                );
                return response;
            }
        }

        public async Task<List<T>> GetUnusedEPpoReceipts<T>(
            string treasuryCode,
            short financialYear
        )
        {
            return await _ePpoReceiptRepository.GetUnusedEPpoReceipts(
                treasuryCode,
                financialYear,
                entity => _mapper.Map<T>(entity)
            );
        }

        public async Task<T> RegisterEPpoReceiptWithdrawal<T>(
            string pensionApplnNo,
            EPpoReceiptWithdrawlEntryDTO ePpoReceiptWithdrawlEntryDTO,
            string treasuryCode,
            short financialYear
        )
        {
            T? response = _mapper.Map<T>(ePpoReceiptWithdrawlEntryDTO);
            EppoReceipt? eppoReceipt = new();
            try
            {
                EppoReceipt? eppoEntity =
                    await _ePpoReceiptRepository.GetEPpoReceiptByPensionApplnNo(
                        pensionApplnNo,
                        treasuryCode,
                        financialYear
                    );

                if (eppoEntity is null)
                {
                    response.FillDataSource(
                        eppoEntity,
                        "No record found for Pension Application No: " + pensionApplnNo
                    );
                    return response;
                }

                eppoEntity.FillFrom(ePpoReceiptWithdrawlEntryDTO);
                SetUpdatedBy(eppoEntity);

                return await _ePpoReceiptRepository.WithdrawEPpoReceipt<T>(
                    pensionApplnNo,
                    eppoEntity,
                    treasuryCode,
                    financialYear,
                    ePpoReceiptWithdrawlEntryDTO.WithdrawReason,
                    ePpoReceiptWithdrawlEntryDTO.FreshRevisionFlag,
                    entity =>
                        _mapper.Map<T>(
                            new EPpoReceiptWithdrawlResponseDTO { PensionApplnNo = pensionApplnNo }
                        )
                );
            }
            catch (Exception ex)
            {
                response.FillDataSource(
                    eppoReceipt,
                    $"ServiceException: {ex.InnerException?.Message ?? ex.Message}"
                );
                return response;
            }
        }
    }
}
