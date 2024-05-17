using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using CTS_BE.DAL.Entities;
using CTS_BE.DAL.Interfaces;
using CTS_BE.DTOs;
using CTS_BE.Helper;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace CTS_BE.DAL.Repositories
{
    public class ChequeReceivedRepository :  Repository<ChequeReceived, CTSDBContext>, IChequeReceivedRepository
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

        // public async Task<ChequeReceivedDTO?> Insert(string chequeReceivedData)
        // {
        //     var _chequeReceivedData = new NpgsqlParameter("@in_cheque_received_data", NpgsqlTypes.NpgsqlDbType.Jsonb);
        //     var _outputParameter = new NpgsqlParameter("@is_done_out", NpgsqlTypes.NpgsqlDbType.Smallint);
        //     _chequeReceivedData.Value = chequeReceivedData;
        //      _outputParameter.Direction = ParameterDirection.InputOutput;
        //     _outputParameter.Value = 0;
        //      var parameters = new[] {_chequeReceivedData, _outputParameter };
        //     var commandText = "call cts.cheque_received(@in_indent_data,@is_done_out)";
        //     await _cTSDBContext.Database.ExecuteSqlRawAsync(commandText, parameters);
        //     int isDone = (Int16)_outputParameter.Value;
        //     return (isDone == 0) ? null : JSONHelper.JsonToObject<ChequeReceivedDTO>(_chequeReceivedData.Value.ToString());
        // }
    }
}