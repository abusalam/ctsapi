using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using CTS_BE.DAL.Entities;
using CTS_BE.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace CTS_BE.DAL.Repositories
{
    public class ChequeInvoiceDetailRepository : Repository<ChequeInvoiceDetail, CTSDBContext>, IChequeInvoiceDetailRepository
    {
        private CTSDBContext _cTSDBContext;

        public ChequeInvoiceDetailRepository(CTSDBContext context) : base(context)
        {
            _cTSDBContext = context;
        }

        // public async Task<InvoiceDetails> GetInvoiceDetailsByChequeEntryId(int chequeEntryId)
        // {
        //     List<InvoiceDetails> mockInvoiceDetails = new List<InvoiceDetails>
        //     {
        //         new InvoiceDetails;
        //     }
        //     var invoiceDetail = mockInvoiceDetails.FirstOrDefault(d => d.ChequeEntryId == chequeEntryId);
        //     return invoiceDetail;
        // }

        public async Task<bool> Insert(string chequeInvoiceDetailsData)
        {
            var _chequeInvoiceDeatilsData = new NpgsqlParameter("@in_cheque_invoice_details_data", NpgsqlTypes.NpgsqlDbType.Jsonb);
            var _outputParameter = new NpgsqlParameter("@is_done_out", NpgsqlTypes.NpgsqlDbType.Smallint);
            _chequeInvoiceDeatilsData.Value = chequeInvoiceDetailsData;
            _outputParameter.Direction = ParameterDirection.InputOutput;
            _outputParameter.Value = 0;
            var parameters = new[] { _chequeInvoiceDeatilsData, _outputParameter };
            var commandText = "call cts.cheque_invoice_details(@in_cheque_invoice_data,@is_done_out)";
            await _cTSDBContext.Database.ExecuteSqlRawAsync(commandText, parameters);
            int isDone = (Int16)_outputParameter.Value;
            return (isDone == 0) ? false : true;
        }

    }

    public class Repository<T>
    {

    }

}