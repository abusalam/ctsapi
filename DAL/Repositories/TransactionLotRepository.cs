using System.Data;
using CTS_BE.DAL.Entities;
using CTS_BE.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using Npgsql;
namespace CTS_BE.DAL
{
   public class TransactionLotRepository : Repository<TransactionLot, CTSDBContext>, ITransactionLotRepository
   {
        private readonly CTSDBContext _context;

        public TransactionLotRepository(CTSDBContext context) : base(context)
       {
            _context = context;
       }
       public async Task<bool> NewLot(long userId)
       {
          var loggedinUserId = new NpgsqlParameter("@in_loggedin_user_id",NpgsqlTypes.NpgsqlDbType.Bigint);
          var outputParameter = new NpgsqlParameter("@out_is_done", NpgsqlTypes.NpgsqlDbType.Smallint);
          loggedinUserId.Value = userId;
          outputParameter.Direction = ParameterDirection.InputOutput;
          outputParameter.Value = 0;
          var parameters = new[] { loggedinUserId, outputParameter };
          var commandText = "CALL cts_payment.lot_generate(@in_loggedin_user_id,@out_is_done)";
          await _context.Database.ExecuteSqlRawAsync(commandText,parameters);
          int isDone = (Int16)outputParameter.Value;
          return (isDone == 0) ? false : true;
       }
   }
}