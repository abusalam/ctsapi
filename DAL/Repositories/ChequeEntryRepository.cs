using System.Data;
using CTS_BE.DAL.Entities;
using CTS_BE.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using Npgsql;
namespace CTS_BE.DAL
{
    public class ChequeEntryRepository : Repository<ChequeEntry, CTSDBContext>, IChequeEntryRepository
    {
        private readonly CTSDBContext _context;

        public ChequeEntryRepository(CTSDBContext context) : base(context)
        {
            _context = context;
        }
        public async Task<bool> NewChequeEntries(string chequeEntryData)
        {
            var _chequeEntryData = new NpgsqlParameter("@in_cheque_entry_data", NpgsqlTypes.NpgsqlDbType.Jsonb);
            var _outputParameter = new NpgsqlParameter("@is_done_out", NpgsqlTypes.NpgsqlDbType.Smallint);
            _chequeEntryData.Value = chequeEntryData;
            _outputParameter.Direction = ParameterDirection.InputOutput;
            _outputParameter.Value = 0;
            var parameters = new[] { _chequeEntryData, _outputParameter };
            var commandText = "call cts.cheque_entry(@in_cheque_entry_data,@is_done_out)";
            await _context.Database.ExecuteSqlRawAsync(commandText, parameters);
            int isDone = (Int16)_outputParameter.Value;
            return (isDone == 0) ? false : true;
        }
    }
}