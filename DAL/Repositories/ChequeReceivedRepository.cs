using CTS_BE.DAL.Entities;
using CTS_BE.DAL.Interfaces;
using CTS_BE.DTOs;
using CTS_BE.Helper;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Data;

namespace CTS_BE.DAL.Repositories
{
    public class ChequeReceivedRepository : Repository<ChequeReceived, CTSDBContext>, IChequeReceivedRepository
    {
        private readonly CTSDBContext _cTSDBContext;

        public ChequeReceivedRepository(CTSDBContext context) : base(context)
        {
            _cTSDBContext = context;
        }

        public Task<ChequeReceivedDTO> Insert(string chequeReceivedData)
        {
           throw new NotImplementedException();
        }

        public async Task<Int16?> Insert(string chequeReceivedData, string exclusionList ) //series id, //exclusion list 
        {
            var _chequeReceivedData = new NpgsqlParameter("@in_cheque_received_data", NpgsqlTypes.NpgsqlDbType.Jsonb);
            var _exclusion = new NpgsqlParameter("@in_exclusion", NpgsqlTypes.NpgsqlDbType.Jsonb);
            var _outputParameter = new NpgsqlParameter("@is_done_out", NpgsqlTypes.NpgsqlDbType.Smallint);
            _chequeReceivedData.Value = chequeReceivedData;
            _exclusion.Value = exclusionList;
            _outputParameter.Direction = ParameterDirection.InputOutput;
            _outputParameter.Value = 0;
            var parameters = new[] { _chequeReceivedData, _outputParameter, _exclusion};
            var commandText = "call cts.cheque_received(@in_cheque_received_data,@in_exclusion,@is_done_out)";
            await _cTSDBContext.Database.ExecuteSqlRawAsync(commandText, parameters);
            
            return (Int16)_outputParameter.Value;
        }
    }
}