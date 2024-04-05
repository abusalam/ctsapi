using CTS_BE.DAL.Entities;
using CTS_BE.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Data;

namespace CTS_BE.DAL
{
   public class ChequeInvoiceRepository : Repository<ChequeInvoice, CTSDBContext>, IChequeInvoiceRepository
   {
        private readonly CTSDBContext _cTSDBContext;

        public ChequeInvoiceRepository(CTSDBContext context) : base(context)
       {
            _cTSDBContext = context;
        }
        public async Task<bool> Insert(string chequeInvoiceData)
        {
            var _chequeInvoiceData = new NpgsqlParameter("@in_cheque_invoice_data", NpgsqlTypes.NpgsqlDbType.Jsonb);
            var _outputParameter = new NpgsqlParameter("@is_done_out", NpgsqlTypes.NpgsqlDbType.Smallint);
            _chequeInvoiceData.Value = chequeInvoiceData;
            _outputParameter.Direction = ParameterDirection.InputOutput;
            _outputParameter.Value = 0;
            var parameters = new[] { _chequeInvoiceData, _outputParameter };
            var commandText = "call cts.cheque_invoice(@in_cheque_invoice_data,@is_done_out)";
            await _cTSDBContext.Database.ExecuteSqlRawAsync(commandText, parameters);
            int isDone = (Int16)_outputParameter.Value;
            return (isDone == 0) ? false : true;
        }
   }
}