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
        }

        public async Task<bool> IndentApprove(string RaisedToTreasuryCode, int Quantity)
        {
            var _raisedToTreasuryCode = new NpgsqlParameter("@_sender_treasury_code", NpgsqlTypes.NpgsqlDbType.Varchar);
            var _quantity = new NpgsqlParameter("@_stamp_number", NpgsqlTypes.NpgsqlDbType.Smallint);
            _raisedToTreasuryCode.Value = RaisedToTreasuryCode;
            _quantity.Value = Quantity;

            var _is_done_out = new NpgsqlParameter("@_is_done_out", NpgsqlTypes.NpgsqlDbType.Boolean);
            _is_done_out.Direction = ParameterDirection.InputOutput;
            _is_done_out.Value = false;

            var parameters = new[] { _raisedToTreasuryCode, _quantity, _is_done_out };
            var commandText = "CALL master.approve_stamp_indent(@_sender_treasury_code, @_stamp_number, @_is_done_out)";
            await _context.Database.ExecuteSqlRawAsync(commandText, parameters);
            //Boolean isDone = (Boolean)is_done_out.Value;
            return (bool)_is_done_out.Value;
        }
        
        public async Task<bool> IndentRecieve(string RaisedToTreasuryCode, string RaisedByTreasuryCode, int Quantity, long IndentId)
        {
            var _stamp_number = new NpgsqlParameter("@_stamp_number", NpgsqlTypes.NpgsqlDbType.Smallint);
            var _receiver_treasury_code = new NpgsqlParameter("@_receiver_treasury_code", NpgsqlTypes.NpgsqlDbType.Varchar);
            //var _receiver_wallet_id = new NpgsqlParameter("@_receiver_wallet_id", NpgsqlTypes.NpgsqlDbType.Varchar);
            var _sender_treasury_code = new NpgsqlParameter("@_sender_treasury_code", NpgsqlTypes.NpgsqlDbType.Varchar);
            var _indent_id = new NpgsqlParameter("@_indent_id", NpgsqlTypes.NpgsqlDbType.Bigint);

            _stamp_number.Value = Quantity;
            _receiver_treasury_code.Value = RaisedByTreasuryCode;
            //_receiver_wallet_id.Value = "000"; // debug
            _sender_treasury_code.Value = RaisedToTreasuryCode;
            _indent_id.Value = IndentId;

            var _is_done_out = new NpgsqlParameter("@_is_done_out", NpgsqlTypes.NpgsqlDbType.Boolean);
            _is_done_out.Direction = ParameterDirection.InputOutput;
            _is_done_out.Value = false;

            //var parameters = new[] { _stamp_number, _receiver_wallet_id, _receiver_treasury_code, _sender_treasury_code, _indent_id, _is_done_out };
            //var commandText = "CALL master.receive_stamp_indent(@_stamp_number, @_receiver_wallet_id, @_receiver_treasury_code, @_sender_treasury_code, @_indent_id, @_is_done_out)";
            //await _context.Database.ExecuteSqlRawAsync(commandText, parameters);
            
            var parameters = new[] { _indent_id, _sender_treasury_code,  _receiver_treasury_code, _stamp_number, _is_done_out };
            var commandText = "CALL master.receive_stamp_indent(@_indent_id, @_sender_treasury_code,  @_receiver_treasury_code,  @_stamp_number, @_is_done_out)";
            await _context.Database.ExecuteSqlRawAsync(commandText, parameters);
            //Boolean isDone = (Boolean)is_done_out.Value;
            return (bool)_is_done_out.Value;
        }

    }
}
