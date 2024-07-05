using CTS_BE.DAL.Entities;
using CTS_BE.DAL.Interfaces.stamp;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Data;

namespace CTS_BE.DAL.Repositories.stamp
{
    public class StampWalletRepository : Repository<StampWallet, CTSDBContext>, IStampWalletRepository
    {
        protected readonly CTSDBContext _context;
        public StampWalletRepository(CTSDBContext context) : base(context)
        {
            _context = context;
            _context.Set<StampWallet>()
                    .Include(t => t.Combination);
        }

        public async Task<bool> WalletRefil(string TreasuryCode, long CombinationId, short AddSheet, short AddLabel)
        {
            // return type change
            var _treasuryCode = new NpgsqlParameter("@_treasury_code", NpgsqlTypes.NpgsqlDbType.Varchar);
            var _add_sheet = new NpgsqlParameter("@_add_sheet", NpgsqlTypes.NpgsqlDbType.Smallint);
            var _add_label = new NpgsqlParameter("@_add_label", NpgsqlTypes.NpgsqlDbType.Smallint);
            var _combination_id = new NpgsqlParameter("@_combination_id", NpgsqlTypes.NpgsqlDbType.Bigint);

            _treasuryCode.Value = TreasuryCode;
            _add_sheet.Value = AddSheet;
            _add_label.Value = AddLabel;
            _combination_id.Value = CombinationId;

            var _is_done_out = new NpgsqlParameter("@_is_done_out", NpgsqlTypes.NpgsqlDbType.Boolean);
            var _out_message = new NpgsqlParameter("@_out_message", NpgsqlTypes.NpgsqlDbType.Text);

            _is_done_out.Direction = ParameterDirection.InputOutput;
            _is_done_out.Value = false;

            _out_message.Direction = ParameterDirection.InputOutput;
            _out_message.Value = "";

            var parameters = new[] { _treasuryCode, _combination_id, _add_sheet, _add_label, _is_done_out, _out_message };
            var commandText = "CALL master.wallet_refill(@_treasuryCode, @_combination_id, @_add_sheet, @_add_label, @_is_done_out, @_out_message)";
            await _context.Database.ExecuteSqlRawAsync(commandText, parameters);
            return (bool)_is_done_out.Value;
        }
    }
}
