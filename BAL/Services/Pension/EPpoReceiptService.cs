using AutoMapper;
using CTS_BE.BAL.Interfaces.Pension;
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
        public EPpoReceiptService(
            IEPpoReceiptRepository ePpoReceiptRepository,
            IMapper mapper,
            IClaimService claimService
        ) : base(claimService)
        {
            _ePpoReceiptRepository = ePpoReceiptRepository;
            _mapper = mapper;
        }

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
                eppoReceipt = _mapper.Map<EppoReceipt>(ePpoReceiptEntryDTO);
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

        public Task<T> CreateEPpoReceiptRevision<T>(EPpoReceiptRevisionEntryDTO ePpoReceiptRevisionEntryDTO, string treasuryCode, short financialYear)
        {
            throw new NotImplementedException();
        }

        public Task<T> GetEPpoReceiptByApplicationNo<T>(string applicationNo, string treasuryCode, short financialYear)
        {
            throw new NotImplementedException();
        }

        public Task<T> GetPpoIdByPpoNo<T>(string ppoNo, string treasuryCode, short financialYear)
        {
            throw new NotImplementedException();
        }

        public Task<T> RegisterEPpoReceiptWithdrawal<T>(string applicationNo, EPpoReceiptWithdrawlEntryDTO ePpoReceiptWithdrawlEntryDTO, string treasuryCode, short financialYear)
        {
            throw new NotImplementedException();
        }
    }
}