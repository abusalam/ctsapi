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
            T response = _mapper.Map<T>(eppoReceipt);
            try
            {
                eppoReceipt.FinancialYear = financialYear;
                ProcessFiles(eppoReceipt, treasuryCode, financialYear);
                SetCreatedBy(eppoReceipt);

                // Check if EppoReceipt already exists
                EppoReceipt? existingEppoReceipt =
                    await _ePpoReceiptRepository.GetEPpoReceiptByPensionApplnNo(
                        eppoReceipt.PensionApplnNo,
                        treasuryCode,
                        financialYear
                    );

                if (existingEppoReceipt != null)
                {
                    response.FillErrorInDataSource(
                        existingEppoReceipt,
                        "eppoReceipt already exists for Pension Application No: "
                            + eppoReceipt.PensionApplnNo
                    );
                    return response;
                }

                response = await _ePpoReceiptRepository.SaveEPpoReceipt<T>(
                    eppoReceipt,
                    treasuryCode,
                    financialYear,
                    entity => _mapper.Map<T>(entity)
                );
            }
            catch (Exception ex)
            {
                response.FillErrorInDataSource(
                    eppoReceipt,
                    $"ServiceException: {ex.InnerException?.Message ?? ex.Message}"
                );
            }
            return response;
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
                    entity => _mapper.Map<T>(entity)
                );
            }
            catch (Exception ex)
            {
                response.FillErrorInDataSource(
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
                    response.FillErrorInDataSource(
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
                response.FillErrorInDataSource(
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
                    response.FillErrorInDataSource(
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
                response.FillErrorInDataSource(
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
                    response.FillErrorInDataSource(
                        eppoEntity,
                        "No record found for Pension Application No: " + pensionApplnNo
                    );
                    return response;
                }

                eppoEntity.WithdrawDate = DateOnly.FromDateTime(DateTime.UtcNow);
                eppoEntity.TreasuryCode = treasuryCode;
                eppoEntity.FinancialYear = financialYear;

                if (eppoEntity.PpoId == 0)
                {
                    eppoEntity.PpoId = null;
                }

                eppoEntity.PensionApplnNo = pensionApplnNo;

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
                response.FillErrorInDataSource(
                    eppoReceipt,
                    $"ServiceException: {ex.InnerException?.Message ?? ex.Message}"
                );
                return response;
            }
        }

        private void ProcessFiles(EppoReceipt eppoReceipt, string treasuryCode, short financialYear)
        {
            var provider = new FileExtensionContentTypeProvider();

            if (eppoReceipt.EppoFile != null)
            {
                eppoReceipt.EppoFile.FilePath = treasuryCode + "/" + financialYear + "/";
                provider.TryGetContentType(eppoReceipt.EppoFile.FileName, out string? mimeType);
                eppoReceipt.EppoFile.FileMimeType = mimeType ?? "application/octet-stream";
            }

            if (eppoReceipt.PhotoFile != null)
            {
                eppoReceipt.PhotoFile.FilePath = treasuryCode + "/" + financialYear + "/";
                provider.TryGetContentType(eppoReceipt.PhotoFile.FileName, out string? mimeType);
                eppoReceipt.PhotoFile.FileMimeType = mimeType ?? "application/octet-stream";
            }

            if (eppoReceipt.SignatureFile != null)
            {
                eppoReceipt.SignatureFile.FilePath = treasuryCode + "/" + financialYear + "/";
                provider.TryGetContentType(
                    eppoReceipt.SignatureFile.FileName,
                    out string? mimeType
                );
                eppoReceipt.SignatureFile.FileMimeType = mimeType ?? "application/octet-stream";
            }
        }
    }
}
