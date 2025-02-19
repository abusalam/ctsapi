using System;
using System.Linq;
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
    public class PpoByTransferService : BaseService, IPpoByTransferService
    {
        private readonly PensionDbContext _pensionDbContext;
        private readonly IMapper _mapper;
        private readonly IPpoByTransferRepository _ppoByTransferRepository;
        private readonly ITreasuryRepository _treasuryRepository;

        public PpoByTransferService(
            PensionDbContext pensionDbContext,
            IClaimService claimService,
            IMapper mapper,
            IPpoByTransferRepository ppoByTransferRepository,
            ITreasuryRepository treasuryRepository
        )
            : base(claimService)
        {
            _pensionDbContext = pensionDbContext;
            _mapper = mapper;
            _ppoByTransferRepository = ppoByTransferRepository;
            _treasuryRepository = treasuryRepository;
        }

        public async Task<T> SavePpoByTransferHead<T>(
            PpoByTransferEntryDTO ppoByTransferEntryDTO,
            short financialYear,
            string treasuryCode
        )
        {
            PpoBytransfer ppoByTransferEntity = _mapper.Map<PpoBytransfer>(ppoByTransferEntryDTO);
            ppoByTransferEntity.FinancialYear = financialYear;
            ppoByTransferEntity.TreasuryCode = treasuryCode;
            T responseDTO = _mapper.Map<T>(ppoByTransferEntity);

            try
            {
                var treasury = await _treasuryRepository.GetTreasuryNameAsync(treasuryCode);
                if (treasury == null)
                {
                    responseDTO.FillDataSource(
                        ppoByTransferEntity,
                        "Treasury code not found! Please check Treasury Code."
                    );
                    return responseDTO;
                }

                var existingPpoByTransfer = await _pensionDbContext
                    .PpoBytransfers.Where(entity =>
                        entity.ActiveFlag
                        && entity.PensionerId == ppoByTransferEntity.PensionerId
                        && entity.BytransferHeadId == ppoByTransferEntity.BytransferHeadId
                        && entity.FromDate == ppoByTransferEntity.FromDate
                        && entity.ToDate == ppoByTransferEntity.ToDate
                    )
                    .FirstOrDefaultAsync();

                if (existingPpoByTransfer != null)
                {
                    responseDTO.FillDataSource(
                        existingPpoByTransfer,
                        "PpoByTransfer already exists!"
                    );
                    return responseDTO;
                }

                SetCreatedBy(ppoByTransferEntity);
                ppoByTransferEntity.UpdatedAt = DateTime.Now;

                responseDTO = await _ppoByTransferRepository.SavePpoByTransferHead<T>(
                    ppoByTransferEntity
                );
            }
            catch (Exception ex)
            {
                responseDTO.FillDataSource(
                    _mapper.Map<T>(ppoByTransferEntity),
                    $"ServiceException: {ex.InnerException?.Message ?? ex.Message}"
                );
            }

            return responseDTO;
        }
    }
}
