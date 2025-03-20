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
                responseDTO.FillErrorInDataSource(
                    byTransferHeadEntity,
                    "Bytransfer head not saved!"
                );
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
            return await _pensionDbContext
                .BytransferHeads.Where(entity => entity.ActiveFlag)
                .Include(entity => entity.AccountHead) // Eager loading directly in query
                .ToListAsync();
        }
    }
}
