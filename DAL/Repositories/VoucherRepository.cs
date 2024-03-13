using CTS_BE.DAL.Entities;
using CTS_BE.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Data;

namespace CTS_BE.DAL
{
   public class VoucherRepository : Repository<Voucher, CTSDBContext>, IVoucherRepository
   {
        private readonly CTSDBContext _context;

        public VoucherRepository(CTSDBContext context) : base(context)
       {
            _context = context;
       }
        public async Task<bool> NewVoucher(string payload,long userId)
        {
            var _paymandatePaylod = new NpgsqlParameter("@in_paymandate_paylod",NpgsqlTypes.NpgsqlDbType.Jsonb);
            var _loggedinUserId = new NpgsqlParameter("@in_loggedin_user_id",NpgsqlTypes.NpgsqlDbType.Bigint);
            var _outputParameter =new NpgsqlParameter("@out_is_done", NpgsqlTypes.NpgsqlDbType.Smallint);
            _paymandatePaylod.Value = payload;
            _loggedinUserId.Value = userId;
            _outputParameter.Direction = ParameterDirection.Output;
            _outputParameter.Value = 0;
            var parameters = new[] { _loggedinUserId, _paymandatePaylod, _outputParameter };
            var commandText = "CALL cts.paymandate(@in_loggedin_user_id,@in_paymandate_paylod,@out_is_done)";
            await _context.Database.ExecuteSqlRawAsync(commandText,parameters);
            int isDone = (Int16)_outputParameter.Value;
            return (isDone == 0) ? false : true;
        }
   }
}