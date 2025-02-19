using AutoMapper;
using CTS_BE.DAL.Entities.Pension;
using CTS_BE.DAL.Interfaces.Pension;
using CTS_BE.Helper;

namespace CTS_BE.DAL.Repositories.Pension
{
    public class PpoByTransferRepository
        : Repository<PpoBytransfer, PensionDbContext>,
            IPpoByTransferRepository
    {
        private readonly PensionDbContext _pensionDbContext;
        private readonly IMapper _mapper;

        public PpoByTransferRepository(PensionDbContext context, IMapper mapper)
            : base(context)
        {
            _pensionDbContext = context;
            _mapper = mapper;
        }

        public async Task<T> SavePpoByTransferHead<T>(PpoBytransfer ppobyTransferHeadEntity)
        {
            T responseDTO = _mapper.Map<T>(ppobyTransferHeadEntity);

            await _pensionDbContext.PpoBytransfers.AddAsync(ppobyTransferHeadEntity);

            if (await _pensionDbContext.SaveChangesAsync() == 0)
            {
                responseDTO.FillDataSource(
                    ppobyTransferHeadEntity,
                    "Ppo Bytransfer amount not saved!"
                );
                return responseDTO;
            }

            _pensionDbContext
                .Entry(ppobyTransferHeadEntity)
                .Reference(entity => entity.BytransferHead)
                .Load();

            responseDTO = _mapper.Map<T>(ppobyTransferHeadEntity);

            return responseDTO;
        }
    }
}
