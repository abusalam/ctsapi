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
            T response = _mapper.Map<T>(ePpoReceiptRevisionEntryDTO);

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

        public async Task<T> GetEPpoReceiptByPension_Appln_No<T>(
            string pension_appln_no, 
            string treasuryCode, 
            short financialYear)
        {
            try 
            {
                return await _ePpoReceiptRepository.GetEPpoReceiptByPension_Appln_No<T>(
                    pension_appln_no,
                    treasuryCode,
                    financialYear,
                    entity => _mapper.Map<T>(entity)
                );
            }
            catch (Exception ex)
            {
                return _mapper.Map<T>(new { Status = "F", ApplicationNo = pension_appln_no, ErrorCode = ex.Message });
            }
        }

        public async Task<T> GetPpoIdByPpoNo<T>(
            string ppoNo, 
            string treasuryCode, 
            short financialYear)
        {
            try 
            {
                return await _ePpoReceiptRepository.GetPpoIdByPpoNo<T>(
                    ppoNo,
                    treasuryCode,
                    financialYear,
                    entity => _mapper.Map<T>(entity)
                );
            }
            catch (Exception ex)
            {
                return _mapper.Map<T>(new { Status = "F", PpoNo = ppoNo, ErrorCode = ex.Message });
            }
        }

        public async Task<T> RegisterEPpoReceiptWithdrawal<T>(
            string pension_appln_no,
            EPpoReceiptWithdrawlEntryDTO ePpoReceiptWithdrawlEntryDTO,
            string treasuryCode,
            short financialYear)
        {
            EppoReceipt? eppoReceipt = await _context.EppoReceipts.FirstOrDefaultAsync(e => e.PensionApplnNo == pension_appln_no);
            if (eppoReceipt == null)
            {
                return _mapper.Map<T>(new 
                {
                    ApplicationNo = pension_appln_no,
                    Status = "F",
                    ErrorCode = "Application not found",
                    Reason = ePpoReceiptWithdrawlEntryDTO.Reason,
                    Flag = ePpoReceiptWithdrawlEntryDTO.Flag
                });
            }

            try 
            {
                return await _ePpoReceiptRepository.WithdrawEPpoReceipt<T>(
                    pension_appln_no,
                    eppoReceipt,
                    treasuryCode,
                    financialYear,
                    ePpoReceiptWithdrawlEntryDTO.Reason,   
                    ePpoReceiptWithdrawlEntryDTO.Flag,  
                    entity => _mapper.Map<T>(new 
                    {
                        ApplicationNo = pension_appln_no,
                        Status = "S",
                        ErrorCode = (string)null,
                        Reason = ePpoReceiptWithdrawlEntryDTO.Reason,
                        Flag = ePpoReceiptWithdrawlEntryDTO.Flag
                    })
                );
            }
            catch (Exception ex)
            {
                return _mapper.Map<T>(new 
                {
                    ApplicationNo = pension_appln_no,
                    Status = "F",
                    ErrorCode = ex.Message,
                    Reason = ePpoReceiptWithdrawlEntryDTO.Reason,
                    Flag = ePpoReceiptWithdrawlEntryDTO.Flag
                });
            }
        }
    }
}