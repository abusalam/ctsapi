using AutoMapper;
using CTS_BE.BAL.Interfaces.Pension;
using CTS_BE.DAL;
using CTS_BE.DAL.Entities.Pension;
using CTS_BE.DAL.Interfaces.Pension;
using CTS_BE.DTOs;
using CTS_BE.Helper;
using CTS_BE.Helper.Authentication;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.BAL.Services.Pension
{
    public class EPpoReceiptService : BaseService, IEPpoReceiptService
    {
        private readonly IEPpoReceiptRepository _ePpoReceiptRepository;
        private readonly IMapper _mapper;
        private readonly PensionDbContext _context;
        public EPpoReceiptService(
            IEPpoReceiptRepository ePpoReceiptRepository,
            IMapper mapper,
            IClaimService claimService,
            PensionDbContext context
        ) : base(claimService)
        {
            _ePpoReceiptRepository = ePpoReceiptRepository;
            _mapper = mapper;
            _context = context;
        }

        public async Task<T> CreateEPpoReceipt<T>(
            EPpoReceiptEntryDTO ePpoReceiptEntryDTO,
            string treasuryCode,
            short financialYear)
        {
            EppoReceipt eppoReceipt = _mapper.Map<EppoReceipt>(ePpoReceiptEntryDTO);
            T response = _mapper.Map<T>(eppoReceipt);
            try
            {
                eppoReceipt.TreasuryCode = treasuryCode;
                eppoReceipt.FinancialYear = financialYear;
                SetCreatedBy(eppoReceipt);

                response = await _ePpoReceiptRepository.SaveEPpoReceipt<T>(
                    eppoReceipt,
                    treasuryCode,
                    financialYear,
                    entity => _mapper.Map<T>(entity)
                );
            }
            catch (DbUpdateException ex)
            {
                response.FillDataSource(eppoReceipt, $"DbException: {ex.InnerException?.Message ?? ex.Message}");
            }
            catch (Exception ex)
            {
                response.FillDataSource(eppoReceipt, $"ServiceException: {ex.InnerException?.Message ?? ex.Message}");
            }
            return response;
        }

        public async Task<T> CreateEPpoReceiptRevision<T>(
            EPpoReceiptRevisionEntryDTO ePpoReceiptRevisionEntryDTO,
            string treasuryCode,
            short financialYear)
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
            catch (DbUpdateException ex)
            {
                response.FillDataSource(eppoRevision, $"DbException: {ex.InnerException?.Message ?? ex.Message}");
            }
            catch (Exception ex)
            {
                response.FillDataSource(eppoRevision, $"ServiceException: {ex.InnerException?.Message ?? ex.Message}");
            }

            return response;
        }

        public async Task<T> GetEPpoReceiptById<T>(
            long receiptId,
            string treasuryCode,
            short financialYear)
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
            catch (DbUpdateException ex)
            {
                response = _mapper.Map<T>(new EPpoReceiptDetailDTO());
                response.FillDataSource(
                    new EppoReceipt(),
                    $"DbException: {ex.InnerException?.Message ?? ex.Message}"
                );
                return response;
            }
            catch (Exception ex)
            {
                response = _mapper.Map<T>(new EPpoReceiptDetailDTO());
                response.FillDataSource(
                    new EppoReceipt(),
                    $"RepositoryException: {ex.InnerException?.Message ?? ex.Message}"
                );
                return response;
            }
        }

        public async Task<T> GetEPpoReceiptByPensionApplnNo<T>(
            string pensionApplnNo,
            string treasuryCode,
            short financialYear)
        {
            T? response = _mapper.Map<T>(new EppoReceipt() {
                PensionApplnNo = pensionApplnNo
            });
            try
            {
                EppoReceipt? eppoReceipt = await _ePpoReceiptRepository.GetEPpoReceiptByPensionApplnNo(
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
            catch (DbUpdateException ex)
            {
                response.FillDataSource(
                    new EppoReceipt(),
                    $"DbException: {ex.InnerException?.Message ?? ex.Message}"
                );
                return response;
            }
            catch (Exception ex) {
                response.FillDataSource(
                    new EppoReceipt(),
                    $"RepositoryException: {ex.InnerException?.Message ?? ex.Message}"
                );
                return response;
            }
        }

        public async Task<List<T>> GetUnusedEPpoReceipts<T>(
            string treasuryCode,
            short financialYear)
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
            short financialYear)
        {
            T? response =  _mapper.Map<T>(ePpoReceiptWithdrawlEntryDTO);
            EppoReceipt ? eppoReceipt = new();
            try
            {

                EppoReceipt? eppoEntity = await _ePpoReceiptRepository.GetEPpoReceiptByPensionApplnNo(
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
                    entity => _mapper.Map<T>(new EPpoReceiptWithdrawlResponseDTO
                    {
                        PensionApplnNo = pensionApplnNo
                    })
                );


            }
            catch (DbUpdateException ex)
            {
                response.FillDataSource(
                        eppoReceipt,
                        $"DbException: {ex.InnerException?.Message}"
                    );
                return response;
            }
            catch (Exception ex)
            {
                response.FillDataSource(
                        eppoReceipt,
                        $"ServiceException: {ex.InnerException?.Message}"
                    );
                return response;
            }
        }
    }
}