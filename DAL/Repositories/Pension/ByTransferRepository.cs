using AutoMapper;
using CTS_BE.DAL.Entities.Pension;
using CTS_BE.DAL.Interfaces.Pension;
using CTS_BE.Helper;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.DAL.Repositories.Pension
{
    public class ByTransferRepository
        : Repository<BytransferHead, PensionDbContext>,
            IByTransferRepository
    {
        private readonly PensionDbContext _pensionDbContext;
        private readonly IMapper _mapper;

        public ByTransferRepository(PensionDbContext context, IMapper mapper)
            : base(context)
        {
            _pensionDbContext = context;
            _mapper = mapper;
        }

        //public async Task<T> SaveByTransferHead<T>(
        //    BytransferHead byTransferHeadEntity,
        //    short financialYear,
        //    string treasuryCode
        //)
        //{
        //    T responseDTO = _mapper.Map<T>(byTransferHeadEntity);

        //    // Check if the corresponding AccountHead exists
        //    AccountHead? accountHead = await _pensionDbContext
        //        .AccountHeads.Where(entity =>
        //            entity.Id == byTransferHeadEntity.AccountHeadId && entity.ActiveFlag
        //        )
        //        .FirstOrDefaultAsync();

        //    if (accountHead == null)
        //    {
        //        responseDTO.FillDataSource(byTransferHeadEntity, "Account head not found!");
        //        return responseDTO;
        //    }

        //    // Populate additional fields in BytransferHead
        //    byTransferHeadEntity.ActiveFlag = true;
        //    byTransferHeadEntity.CreatedAt = DateTime.Now;
        //    byTransferHeadEntity.UpdatedAt = DateTime.Now;
        //    // byTransferHeadEntity.TreasuryCode = treasuryCode;

        //    // Validate and prepare related entities
        //    byTransferHeadEntity
        //        .BillBytransfers.ToList()
        //        .ForEach(billBytransfer =>
        //        {
        //            billBytransfer.ActiveFlag = true;
        //            billBytransfer.FinancialYear = financialYear;
        //            billBytransfer.TreasuryCode = treasuryCode;
        //            billBytransfer.CreatedAt = DateTime.Now;
        //            billBytransfer.CreatedBy = byTransferHeadEntity.CreatedBy;
        //        });

        //    // Add the BytransferHead entity to the DbContext
        //    await _pensionDbContext.BytransferHeads.AddAsync(byTransferHeadEntity);

        //    // Save changes and handle response
        //    if (await _pensionDbContext.SaveChangesAsync() == 0)
        //    {
        //        responseDTO.FillDataSource(byTransferHeadEntity, "Bytransfer head not saved!");
        //        return responseDTO;
        //    }

        //    // Load navigation properties for a complete response
        //    _pensionDbContext
        //        .Entry(byTransferHeadEntity)
        //        .Reference(entity => entity.AccountHead)
        //        .Load();
        //    byTransferHeadEntity
        //        .BillBytransfers.ToList()
        //        .ForEach(billBytransfer =>
        //        {
        //            _pensionDbContext
        //                .Entry(billBytransfer)
        //                .Reference(entity => entity.BytransferHead)
        //                .Load();
        //        });

        //    // Map the updated entity back to DTO
        //    responseDTO = _mapper.Map<T>(byTransferHeadEntity);

        //    return responseDTO;
        //}



        //new added


        public async Task<T> SaveByTransferHead<T>(
            BytransferHead byTransferHeadEntity,
            short financialYear,
            string treasuryCode
        )
        {
            T responseDTO = _mapper.Map<T>(byTransferHeadEntity);

            // Add the BytransferHead entity to the DbContext
            await _pensionDbContext.BytransferHeads.AddAsync(byTransferHeadEntity);

            // Save changes and handle response
            if (await _pensionDbContext.SaveChangesAsync() == 0)
            {
                responseDTO.FillDataSource(byTransferHeadEntity, "Bytransfer head not saved!");
                return responseDTO;
            }

            // Load navigation properties for a complete response
            _pensionDbContext
                .Entry(byTransferHeadEntity)
                .Reference(entity => entity.AccountHead)
                .Load();
            foreach (var billBytransfer in byTransferHeadEntity.BillBytransfers)
            {
                _pensionDbContext
                    .Entry(billBytransfer)
                    .Reference(entity => entity.BytransferHead)
                    .Load();
            }

            // Map the updated entity back to DTO
            responseDTO = _mapper.Map<T>(byTransferHeadEntity);

            return responseDTO;
        }
    }
}
