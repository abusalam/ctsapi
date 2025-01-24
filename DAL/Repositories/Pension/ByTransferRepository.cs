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

        public async Task<BytransferHead?> GetByTransferHeadByIdAsync(
      long byTransferHeadId,
      string treasuryCode
  )
        {
           
            var byTransferHead = await _pensionDbContext
                .BytransferHeads.Where(entity =>
                    entity.ActiveFlag
                    && entity.Id == byTransferHeadId
                    && entity.BillBytransfers.Any(b => b.TreasuryCode == treasuryCode) 
                )
                .FirstOrDefaultAsync();

            if (byTransferHead == null)
            {
                return null;
            }

          
            _pensionDbContext
                .Entry(byTransferHead)
                .Reference(entity => entity.AccountHead) 
                .Load();

            _pensionDbContext
                .Entry(byTransferHead)
                .Collection(entity => entity.BillBytransfers) 
                .Load();

            
            foreach (var billBytransfer in byTransferHead.BillBytransfers)
            {
                _pensionDbContext.Entry(billBytransfer)
                    .Reference(entity => entity.PpoBill) 
                    .Load();
            }

            return byTransferHead;
        }
        public async Task<T> SaveByTransferHead<T>(
            BytransferHead byTransferHeadEntity,
            short financialYear,
            string treasuryCode
        )
        {
            T responseDTO = _mapper.Map<T>(byTransferHeadEntity);

           
            await _pensionDbContext.BytransferHeads.AddAsync(byTransferHeadEntity);

            
            if (await _pensionDbContext.SaveChangesAsync() == 0)
            {
                responseDTO.FillDataSource(byTransferHeadEntity, "Bytransfer head not saved!");
                return responseDTO;
            }

          
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

           
            responseDTO = _mapper.Map<T>(byTransferHeadEntity);

            return responseDTO;
        }
    }
}
