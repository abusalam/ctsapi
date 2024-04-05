using CTS_BE.DAL.Entities;
using CTS_BE.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Data;

namespace CTS_BE.DAL
{
   public class ChequeIndentRepository : Repository<ChequeIndent, CTSDBContext>, IChequeIndentRepository
   {
        private readonly CTSDBContext _cTSDBContext;

        public ChequeIndentRepository(CTSDBContext context) : base(context)
       {
            _cTSDBContext = context;
       }
        public async Task<bool> InsertIndent(string indentData)
        {
            var _indentData = new NpgsqlParameter("@in_indent_data", NpgsqlTypes.NpgsqlDbType.Jsonb);
            var _outputParameter = new NpgsqlParameter("@is_done_out", NpgsqlTypes.NpgsqlDbType.Smallint);
            _indentData.Value = indentData;
            _outputParameter.Direction = ParameterDirection.InputOutput;
            _outputParameter.Value = 0;
            var parameters = new[] {_indentData, _outputParameter };
            var commandText = "call cts.cheque_indent(@in_indent_data,@is_done_out)";
            await _cTSDBContext.Database.ExecuteSqlRawAsync(commandText, parameters);
            int isDone = (Int16)_outputParameter.Value;
            return (isDone == 0) ? false : true;
        }
   }
}