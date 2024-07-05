using CTS_BE.DAL.Entities;
using CTS_BE.DAL.Interfaces.stamp;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Data;

namespace CTS_BE.DAL.Repositories.stamp
{
    public class StampIndentRepository : Repository<StampIndent, CTSDBContext>, IStampIndentRepository
    {
        protected readonly CTSDBContext _context;
        public StampIndentRepository(CTSDBContext context) : base(context)
        {
            _context = context;
            _context.Set<StampIndent>()
                .Include(t => t.StampCombination);
            _context.Set<StampIndent>()
                .Include(t => t.Status);

        }

        public async Task<bool> IndentApprove(string RaisedToTreasuryCode, short sheet, short label, long combinationId)
        {
            var _raisedToTreasuryCode = new NpgsqlParameter("@_sender_treasury_code", NpgsqlTypes.NpgsqlDbType.Varchar);
            var _label_number = new NpgsqlParameter("@_label_number", NpgsqlTypes.NpgsqlDbType.Smallint);
            var _sheet_number = new NpgsqlParameter("@_sheet_number", NpgsqlTypes.NpgsqlDbType.Smallint);
            var _combination_id = new NpgsqlParameter("@_combination_id", NpgsqlTypes.NpgsqlDbType.Bigint);
            var _is_done_out = new NpgsqlParameter("@_is_done_out", NpgsqlTypes.NpgsqlDbType.Boolean);
            var _out_message = new NpgsqlParameter("@_out_message", NpgsqlTypes.NpgsqlDbType.Text);
            
            _is_done_out.Direction = ParameterDirection.InputOutput;
            _out_message.Direction = ParameterDirection.InputOutput;


            _raisedToTreasuryCode.Value = RaisedToTreasuryCode;
            _label_number.Value = label;
            _sheet_number.Value = sheet;
            _combination_id.Value = combinationId;
            _out_message.Value = "";
            _is_done_out.Value = false;


            var parameters = new[] { _raisedToTreasuryCode, _sheet_number, _label_number, _combination_id, _is_done_out, _out_message };
            var commandText = "CALL master.approve_stamp_indent(@_sender_treasury_code, @_sheet_number, @_label_number, @_combination_id, @_is_done_out, @_out_message)";
            await _context.Database.ExecuteSqlRawAsync(commandText, parameters);
            //Boolean isDone = (Boolean)is_done_out.Value;
            return (bool)_is_done_out.Value;
        }
        
        public async Task<bool> IndentRecieve(short sheet, short label, long IndentId)
        {
            var _label_number = new NpgsqlParameter("@_label_number", NpgsqlTypes.NpgsqlDbType.Smallint);
            var _sheet_number = new NpgsqlParameter("@_sheet_number", NpgsqlTypes.NpgsqlDbType.Smallint);
            var _indent_id = new NpgsqlParameter("@_indent_id", NpgsqlTypes.NpgsqlDbType.Bigint);

            _label_number.Value = label;
            _sheet_number.Value = sheet;
            _indent_id.Value = IndentId;

            var _is_done_out = new NpgsqlParameter("@_is_done_out", NpgsqlTypes.NpgsqlDbType.Boolean);
            _is_done_out.Direction = ParameterDirection.InputOutput;
            _is_done_out.Value = false;

            var _out_message = new NpgsqlParameter("@_out_message", NpgsqlTypes.NpgsqlDbType.Text);
            _out_message.Direction = ParameterDirection.InputOutput;
            _out_message.Value = "";

            var parameters = new[] { _indent_id, _sheet_number, _label_number, _is_done_out, _out_message };
            var commandText = "CALL master.receive_stamp_indent(@_indent_id, @_sheet_number, @_label_number, @_is_done_out, @_out_message)";
            await _context.Database.ExecuteSqlRawAsync(commandText, parameters);
            //Boolean isDone = (Boolean)is_done_out.Value;
            return (bool)_is_done_out.Value;
        }

    }
}
