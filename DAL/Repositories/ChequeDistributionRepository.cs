using CTS_BE.DAL.Entities;
using CTS_BE.DAL.Interfaces;
using CTS_BE.DTOs;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace CTS_BE.DAL.Repositories
{
    public class ChequeDistributionRepository : Repository<ChequeDistribute, CTSDBContext>, IChequeDistributionRepository
    {
        private readonly CTSDBContext _context;

        public ChequeDistributionRepository(CTSDBContext context) : base(context)
        {
            _context = context;
        }
        public async Task<bool>Insert(string chequeDistributionDetails)
        {
            var _chequeDistributeData = new NpgsqlParameter("@in_cheque_distribute_data", NpgsqlTypes.NpgsqlDbType.Jsonb);
            var _outputParameter = new NpgsqlParameter("@is_done_out", NpgsqlTypes.NpgsqlDbType.Smallint);
            _chequeDistributeData.Value = chequeDistributionDetails;
            _outputParameter.Direction = ParameterDirection.InputOutput;
            _outputParameter.Value = 0;
            var parameters = new[] { _chequeDistributeData, _outputParameter };
            var commandText = "call cts.cheque_distribute(@in_cheque_distribute_data, @is_done_out)";
            await _context.Database.ExecuteSqlRawAsync(commandText, parameters);
            int isDone = (Int16)_outputParameter.Value;
            return (isDone == 0) ? false : true;


        }
    }
}