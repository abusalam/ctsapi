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

        public async Task<BytransferHead?> GetByTransferHeadByIdAsync(long id)
        {
            var byTransferHead = await _pensionDbContext
                .BytransferHeads.Where(entity => entity.Id == id && entity.ActiveFlag)
                .FirstOrDefaultAsync();

            if (byTransferHead == null)
            {
                return null;
            }

            await _pensionDbContext
                .Entry(byTransferHead)
                .Reference(entity => entity.AccountHead)
                .LoadAsync();

            return byTransferHead;
        }

        public async Task<T> SaveByTransferHead<T>(BytransferHead byTransferHeadEntity)
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

            responseDTO = _mapper.Map<T>(byTransferHeadEntity);

            return responseDTO;
        }

        public async Task<List<BytransferHead>> GetAllByTransferHeadsAsync()
        {
            var byTransferHeads = await _pensionDbContext
                .BytransferHeads.Where(entity => entity.ActiveFlag)
                .ToListAsync();

            if (!byTransferHeads.Any())
            {
                return new List<BytransferHead>();
            }

            // Eager loading
            _pensionDbContext
                .BytransferHeads.Include(entity => entity.AccountHead)
                .Include(entity => entity.BillBytransfers)
                .Include(entity => entity.PpoBytransfers)
                .Load();

            // Explicit loading
            foreach (var byTransferHead in byTransferHeads)
            {
                _pensionDbContext
                    .Entry(byTransferHead)
                    .Reference(entity => entity.AccountHead)
                    .Load();

                _pensionDbContext
                    .Entry(byTransferHead)
                    .Collection(entity => entity.BillBytransfers)
                    .Load();

                _pensionDbContext
                    .Entry(byTransferHead)
                    .Collection(entity => entity.PpoBytransfers)
                    .Load();
            }

            return byTransferHeads;
        }

        //public async Task<List<BytransferHead>> GetAllByTransferHeadsAsync()
        //{
        //    var byTransferHeads = await _pensionDbContext
        //        .BytransferHeads.Where(entity => entity.ActiveFlag)
        //        .ToListAsync();

        //    foreach (var byTransferHead in byTransferHeads)
        //    {
        //        await _pensionDbContext
        //            .Entry(byTransferHead)
        //            .Reference(entity => entity.AccountHead)
        //            .LoadAsync();
        //    }

        //    return byTransferHeads;
        //}
    }
}
