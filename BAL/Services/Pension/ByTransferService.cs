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
    public class ByTransferService : BaseService, IByTransferService
    {
        private readonly PensionDbContext _pensionDbContext;
        private readonly IMapper _mapper;
        private readonly IByTransferRepository _byTransferHeadRepository;
        private readonly ITreasuryRepository _treasuryRepository;

        public ByTransferService(
            PensionDbContext pensionDbContext,
            IClaimService claimService,
            IMapper mapper,
            IByTransferRepository byTransferHeadRepository,
            ITreasuryRepository treasuryRepository
        )
            : base(claimService)
        {
            _pensionDbContext = pensionDbContext;
            _mapper = mapper;
            _byTransferHeadRepository = byTransferHeadRepository;
            _treasuryRepository = treasuryRepository;
        }

     
        public async Task<T> SaveByTransferHead<T>(
            ByTransferHeadEntryDTO byTransferHeadEntryDTO,
            short financialYear,
            string treasuryCode
        )
        {
            BytransferHead byTransferHeadEntity = _mapper.Map<BytransferHead>(
                byTransferHeadEntryDTO
            );
            T responseDTO = _mapper.Map<T>(byTransferHeadEntity);

            try
            {
                // Check if AccountHead exists
                AccountHead? accountHead = await _pensionDbContext
                    .AccountHeads.Where(entity =>
                        entity.Id == byTransferHeadEntity.AccountHeadId && entity.ActiveFlag
                    )
                    .FirstOrDefaultAsync();

                if (accountHead == null)
                {
                    responseDTO.FillDataSource(
                        byTransferHeadEntity,
                        "Account head not found! Please check Account Head ID."
                    );
                    return responseDTO;
                }

                // Check if ByTransferHead already exists
                BytransferHead? existingByTransferHead = await _pensionDbContext
                    .BytransferHeads.Where(entity =>
                        entity.ActiveFlag
                        && entity.AccountHeadId == byTransferHeadEntity.AccountHeadId
                        && entity.BytransferType == byTransferHeadEntity.BytransferType
                        && entity.BytransferDescription
                            == byTransferHeadEntity.BytransferDescription
                    )
                    .FirstOrDefaultAsync();

                if (existingByTransferHead != null)
                {
                    responseDTO = _mapper.Map<T>(existingByTransferHead);
                    responseDTO.FillDataSource(
                        existingByTransferHead,
                        "ByTransferHead already exists! Please check the description and type."
                    );
                    return responseDTO;
                }

               
                SetCreatedBy(byTransferHeadEntity);
                byTransferHeadEntity.UpdatedAt = DateTime.Now;
                // byTransferHeadEntity.TreasuryCode = treasuryCode;

                // Process related entities (BillBytransfers)
                foreach (var billBytransfer in byTransferHeadEntity.BillBytransfers)
                {
                    SetCreatedBy(billBytransfer);
                    billBytransfer.FinancialYear = financialYear;
                    billBytransfer.TreasuryCode = treasuryCode;
                }

                // Save ByTransferHead using the repository
                responseDTO = await _byTransferHeadRepository.SaveByTransferHead<T>(
                    byTransferHeadEntity,
                    financialYear,
                    treasuryCode
                );
            }
            catch (Exception ex)
            {
                responseDTO.FillDataSource(
                    _mapper.Map<T>(byTransferHeadEntity),
                    $"ServiceException: {ex.InnerException?.Message ?? ex.Message}"
                );
            }

            return responseDTO;
        }
    }
}
