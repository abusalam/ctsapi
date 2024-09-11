using AutoMapper;
using CTS_BE.BAL.Interfaces.Pension;
using CTS_BE.DAL;
using CTS_BE.DAL.Entities.Pension;
using CTS_BE.DAL.Interfaces.Pension;
using CTS_BE.DTOs;
using CTS_BE.Helper;
using CTS_BE.Helper.Authentication;
using CTS_BE.PensionEnum;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.BAL.Services.Pension
{
    public class PpoBillService : BaseService, IPpoBillService
    {
        private readonly IMapper _mapper;
        private readonly IPpoBillRepository _ppoBillRepository;
        private readonly PensionDbContext _pensionDbContext;
        public PpoBillService(
            IPpoBillRepository ppoBillRepository,
            IMapper mapper,
            IClaimService claimService
        ) : base(claimService)
        {
            _mapper = mapper;
            _ppoBillRepository = ppoBillRepository;
            _pensionDbContext = (PensionDbContext) this._ppoBillRepository.GetDbContext();
        }

        public async Task<T> GetAllPposForBillGeneration<T>(
            short year,
            short month,
            char billType,
            short financialYear,
            string treasuryCode
        ) where T : BaseDTO
        {

            PpoListResponseDTO ppoListResponseDTO = new();

            try {

                var ppoList = await _pensionDbContext.Pensioners
                .Where(
                    entity => entity.ActiveFlag
                    && entity.TreasuryCode == treasuryCode
                    && entity.PpoStatusFlags.Any(
                        entity => entity.ActiveFlag
                        && entity.StatusFlag == PpoStatus.PpoRunning
                    )
                    && entity.PpoComponentRevisions.Any(
                        entity => entity.ActiveFlag
                    )
                )
                .Include(
                    entity => entity.PpoStatusFlags
                    .Where(
                        entity => entity.ActiveFlag
                    )
                )
                .Include(
                    entity => entity.PpoComponentRevisions
                    .Where(
                        entity => entity.ActiveFlag
                    )
                )
                .ToListAsync();

                ppoListResponseDTO.PpoList = _mapper.Map<List<PensionerListItemDTO>>(ppoList);
            }
            catch (Exception ex) {
                T? ppoBillResponseDTO = _mapper.Map<T>(ppoListResponseDTO);
                ppoBillResponseDTO.FillDataSource(
                    ppoBillResponseDTO,
                    $"ServiceException: {ex.InnerException?.Message ?? ex.Message}"
                );
                return ppoBillResponseDTO;
            }

            return _mapper.Map<T>(ppoListResponseDTO);
        }

        public async Task<T> SavePpoBill<T>(
            PensionerFirstBillResponseDTO firstBill,
            short financialYear,
            string treasuryCode
        ) where T : PpoBillResponseDTO
        {
            PpoBill ppoBillEntity = _mapper.Map<PpoBill>(firstBill);
            T ppoBillResponseDTO = _mapper.Map<T>(ppoBillEntity);
            try {

                PpoBill? ppoBill = await _pensionDbContext.PpoBills
                    .Where(
                        entity => entity.ActiveFlag
                        && entity.PpoId == firstBill.PpoId
                        && entity.BillType == firstBill.BillType
                        // && entity.BillDate == billDate
                        // && entity.FinancialYear == financialYear
                        && entity.TreasuryCode == treasuryCode
                    )
                    // .Select(
                    //     entity => _mapper.Map<PpoBill>(entity)
                    // )
                    .FirstOrDefaultAsync();
                // Console.WriteLine(JsonConvert.SerializeObject(ppoBill, Formatting.Indented));
                if(ppoBill != null) {
                    ppoBillResponseDTO = _mapper.Map<T>(ppoBill);
                    ppoBillResponseDTO.FillDataSource(
                        ppoBill,
                        "Bill already exists! Please check PPO ID, bill date and bill type."
                    );
                    return ppoBillResponseDTO;
                }

                SetCreatedBy(ppoBillEntity);
                ppoBillResponseDTO = await _ppoBillRepository.SavePpoBill<T>(
                        ppoBillEntity,
                        financialYear,
                        treasuryCode
                    );
                ppoBillResponseDTO.PreparedBy = GetUserName();
                ppoBillResponseDTO.PreparedOn = DateOnly.FromDateTime(DateTime.Now);
            }
            catch (DbUpdateException ex) {
                ppoBillResponseDTO.FillDataSource(
                    _mapper.Map<T>(ppoBillEntity),
                    $"DbUpdateException: {ex.InnerException?.Message} {this.ToString()}"
                );
                return ppoBillResponseDTO;
            }
            catch (Exception ex) {
                ppoBillResponseDTO.FillDataSource(
                    _mapper.Map<T>(ppoBillEntity),
                    $"ServiceException: {ex.InnerException?.Message ?? ex.Message} {this.ToString()}"
                );
                return ppoBillResponseDTO;
            }
            return ppoBillResponseDTO;
        }

        public async Task<PpoBillResponseDTO> GetFirstBillByPpoId(
            int ppoId,
            short financialYear,
            string treasuryCode
        )
        {
            PpoBill? ppoBillEntity = new ();
            PpoBillResponseDTO ppoBillResponseDTO = _mapper.Map<PpoBillResponseDTO>(ppoBillEntity);
            try {

                ppoBillEntity = await _ppoBillRepository.GetPpoFirstBillByPpoId(
                    ppoId,
                    financialYear,
                    treasuryCode
                );
                if(ppoBillEntity == null) {
                    ppoBillResponseDTO.FillDataSource(
                        ppoBillEntity,
                        "Bill not found! Please add bill first or check PPO id."
                    );
                    return ppoBillResponseDTO;
                }
                ppoBillResponseDTO = _mapper.Map<PpoBillResponseDTO>(ppoBillEntity);
                ppoBillResponseDTO.PreparedBy = GetUserName();
                ppoBillResponseDTO.PreparedOn = DateOnly.FromDateTime(DateTime.Now);
                return ppoBillResponseDTO;
            }
            catch (DbUpdateException ex) {
                ppoBillResponseDTO.FillDataSource(
                    _mapper.Map<PpoBillResponseDTO>(ppoBillEntity),
                    $"DbUpdateException: {ex.InnerException?.Message} {this.ToString()}"
                );
                return ppoBillResponseDTO;
            }
            catch (Exception ex) {
                ppoBillResponseDTO.FillDataSource(
                    _mapper.Map<PpoBillResponseDTO>(ppoBillEntity),
                    $"ServiceException: {ex.InnerException?.Message ?? ex.Message} {this.ToString()}"
                );
                return ppoBillResponseDTO;
            }
        }
    }
}