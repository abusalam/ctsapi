using System.Linq.Expressions;
using AutoMapper;
using CTS_BE.DAL.Entities.Pension;
using CTS_BE.DAL.Interfaces.Pension;
using CTS_BE.Helper;
using Microsoft.EntityFrameworkCore;

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

        public async Task<PpoBytransfer?> GetPPOByTransferByIdAsync(long id)
        {
            var ppoByTransfer = await _pensionDbContext
                .PpoBytransfers.Where(entity => entity.Id == id && entity.ActiveFlag)
                .FirstOrDefaultAsync();

            if (ppoByTransfer == null)
            {
                return null;
            }

            //await _pensionDbContext
            //    .Entry(ppoByTransfer)
            //    .Reference(entity => entity.AccountHead)
            //    .LoadAsync();

            return ppoByTransfer;
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

        public async Task<T> UpdatePPOByTransfer<T>(PpoBytransfer ppoByTransferEntity)
        {
            T responseDTO = _mapper.Map<T>(ppoByTransferEntity);

            _pensionDbContext.PpoBytransfers.Update(ppoByTransferEntity);

            if (await _pensionDbContext.SaveChangesAsync() == 0)
            {
                responseDTO.FillDataSource(
                    ppoByTransferEntity,
                    "PPO By Transfer record not updated!"
                );
                return responseDTO;
            }

            _pensionDbContext
                .Entry(ppoByTransferEntity)
                .Reference(entity => entity.BytransferHead)
                .Load();

            responseDTO = _mapper.Map<T>(ppoByTransferEntity);

            return responseDTO;
        }

        public async Task<List<T>?> GetAllPpoByTransferByPpoIdAsync<T>(
            int ppoId,
            short financialYear,
            string treasuryCode,
            Expression<Func<PpoBytransfer, T>> selectExpression
        )
        {
            List<T>? ppobytransferList = await _pensionDbContext
                .PpoBytransfers.Where(entity =>
                    entity.ActiveFlag
                    && entity.PpoId == ppoId
                    && entity.FinancialYear == financialYear
                    && entity.TreasuryCode == treasuryCode
                )
                .Include(entity => entity.Pensioner)
                .Include(entity => entity.BytransferHead)
                .Select(selectExpression)
                .ToListAsync();

            return ppobytransferList;
        }

        //IsUsedInOtherTables
        public async Task<bool> IsUsedInOtherTables(long byTransferHeadId)
        {
            bool isUsed = await _pensionDbContext.BillBytransfers.AnyAsync(b =>
                b.BytransferHeadId == byTransferHeadId
            );

            return isUsed;
        }
    }
}
