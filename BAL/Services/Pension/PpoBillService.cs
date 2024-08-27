using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
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

        public async Task<PpoBillResponseDTO> SaveFirstBill(
            PpoBillEntryDTO ppoBillEntryDTO,
            short financialYear,
            string treasuryCode
        )
        {
            PpoBill ppoBillEntity = new ();
            PpoBillResponseDTO ppoBillResponseDTO = _mapper.Map<PpoBillResponseDTO>(ppoBillEntryDTO);
            try {
                ppoBillEntity.FillFrom(ppoBillEntryDTO);

                ppoBillEntity.FinancialYear = financialYear;
                ppoBillEntity.TreasuryCode = treasuryCode;
                ppoBillEntity.BillNo = await _ppoBillRepository.GetNextBillNo((short)ppoBillEntity.FinancialYear, ppoBillEntity.TreasuryCode);
                ppoBillEntity.BillType = 'F';

                SetCreatedBy(ppoBillEntity);
                
                List<PpoBillBreakup> ppoBillBreakups = new ();

                ppoBillEntryDTO.Breakups.ForEach(breakup => {
                    PpoBillBreakup ppoBillBreakupEntity = new ();
                    ppoBillBreakupEntity.FillFrom(breakup);
                    // ppoBillBreakupEntity.BillId = ppoBillEntity.Id;
                    ppoBillBreakupEntity.TreasuryCode = treasuryCode;
                    ppoBillBreakupEntity.FinancialYear = financialYear;
                    SetCreatedBy(ppoBillBreakupEntity);
                    ppoBillBreakups.Add(ppoBillBreakupEntity);
                });

                ppoBillEntity.PpoBillBreakups = ppoBillBreakups;
                
                _ppoBillRepository.Add(ppoBillEntity);

                if(await _ppoBillRepository.SaveChangesManagedAsync()==0) {
                    ppoBillResponseDTO.FillDataSource(
                        _mapper.Map<PpoBillResponseDTO>(ppoBillEntity),
                        "Bill not saved!"
                    );
                    return ppoBillResponseDTO;
                }
                // ppoBillEntity = await _ppoBillRepository.SavePpoBillBreakups(
                //     ppoBillEntity.Id,
                //     ppoBillBreakups
                //     );

            }
            catch (DbUpdateException ex) {
                ppoBillResponseDTO.FillDataSource(
                    _mapper.Map<PpoBillResponseDTO>(ppoBillEntity),
                    $"DbUpdateException: {ex.InnerException?.Message}"
                );
                return ppoBillResponseDTO;
            }
            finally {
                if(ppoBillEntity.Id > 0) {
                    _pensionDbContext.Entry(ppoBillEntity)
                        .Reference(entity => entity.BankAccount).Load();
                }
            }
            return _mapper.Map<PpoBillResponseDTO>(ppoBillEntity);
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
                return ppoBillResponseDTO;
            }
            catch (DbUpdateException ex) {
                ppoBillResponseDTO.FillDataSource(
                    _mapper.Map<PpoBillResponseDTO>(ppoBillEntity),
                    $"DbUpdateException: {ex.InnerException?.Message}"
                );
                return ppoBillResponseDTO;
            }
            finally {

            }
        }
    }
}