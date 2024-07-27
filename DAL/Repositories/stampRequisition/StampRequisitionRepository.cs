using CTS_BE.DAL.Entities;
using CTS_BE.DAL.Interfaces.stampRequisition;
using CTS_BE.DTOs;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Data;

namespace CTS_BE.DAL.Repositories.stampRequisition
{
    public class StampRequisitionRepository : Repository<VendorStampRequisition, CTSDBContext>, IStampRequisitionRepository
    {
        protected readonly CTSDBContext _context;
        public StampRequisitionRepository(CTSDBContext context) : base(context)
        {
            _context = context;
            _context.Set<VendorStampRequisition>()
                .Include(t => t.VendorRequisitionApprove)
                .Include(t => t.Vendor);
        }

        public async Task<bool> ApproveByStampClerk(long vendorStampRequisitionId, short sheet, short label)
        {
            var _label_number = new NpgsqlParameter("@_label_number", NpgsqlTypes.NpgsqlDbType.Smallint);
            var _sheet_number = new NpgsqlParameter("@_sheet_number", NpgsqlTypes.NpgsqlDbType.Smallint);
            var _vendor_stamp_requisition_id = new NpgsqlParameter("@_vendor_stamp_requisition_id", NpgsqlTypes.NpgsqlDbType.Bigint);
            var _is_done_out = new NpgsqlParameter("@_is_done_out", NpgsqlTypes.NpgsqlDbType.Boolean);

            _is_done_out.Direction = ParameterDirection.InputOutput;

            _vendor_stamp_requisition_id.Value = vendorStampRequisitionId;
            _label_number.Value = label;
            _sheet_number.Value = sheet;
            _is_done_out.Value = false;


            var parameters = new[] { _vendor_stamp_requisition_id, _sheet_number, _label_number,_is_done_out};
            var commandText = "CALL cts.approve_by_stamp_clerk(@_vendor_stamp_requisition_id, @_sheet_number, @_label_number, @_is_done_out)";
            await _context.Database.ExecuteSqlRawAsync(commandText, parameters);
            return (bool)_is_done_out.Value;
        }
        public async Task<bool> ApproveByTO(StampRequisitionApprovedByTODTO stampRequisition)
        {
            //public string RequisitionNo { get; set; }
            //public string Head { get; set; }
            var _vendor_requisition_staging_id = new NpgsqlParameter("@_vendor_requisition_staging_id", NpgsqlTypes.NpgsqlDbType.Bigint);
            var _hoa_id = new NpgsqlParameter("@_hoa_id", NpgsqlTypes.NpgsqlDbType.Bigint);
            var _sheet_number = new NpgsqlParameter("@_sheet_number", NpgsqlTypes.NpgsqlDbType.Smallint);
            var _label_number = new NpgsqlParameter("@_label_number", NpgsqlTypes.NpgsqlDbType.Smallint);
            var _discounted_amount = new NpgsqlParameter("@_discounted_amount", NpgsqlTypes.NpgsqlDbType.Numeric);
            var _tax_amount = new NpgsqlParameter("@_tax_amount", NpgsqlTypes.NpgsqlDbType.Numeric);
            var _hoa = new NpgsqlParameter("@_hoa", NpgsqlTypes.NpgsqlDbType.Varchar);
            var _requisition_no = new NpgsqlParameter("@_requisition_no", NpgsqlTypes.NpgsqlDbType.Varchar);
            var _is_done_out = new NpgsqlParameter("@_is_done_out", NpgsqlTypes.NpgsqlDbType.Boolean);

            _is_done_out.Direction = ParameterDirection.InputOutput;

            _vendor_requisition_staging_id.Value = stampRequisition.VendorRequisitionStagingId;
            _hoa_id.Value = stampRequisition.HoaId;
            _sheet_number.Value = stampRequisition.SheetByTo;
            _label_number.Value = stampRequisition.LabelByTo;
            _discounted_amount.Value = stampRequisition.DiscountedAmount;
            _tax_amount.Value = stampRequisition.TaxAmount;
            _requisition_no.Value = stampRequisition.RequisitionNo;
            _hoa.Value = stampRequisition.Head;
            _is_done_out.Value = false;


            var parameters = new[] { _vendor_requisition_staging_id, _hoa_id, _sheet_number, _label_number, _discounted_amount, _tax_amount, _requisition_no, _hoa, _is_done_out };
            var commandText = "CALL cts.approve_by_treasury_officer( @_vendor_requisition_staging_id, @_hoa_id, @_sheet_number, @_label_number, @_discounted_amount, @_tax_amount, @_requisition_no, @_hoa, @_is_done_out )";
            await _context.Database.ExecuteSqlRawAsync(commandText, parameters);
            return (bool)_is_done_out.Value;
        }
    }
}