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
        }

        async Task<bool> IStampWalletRepository.WalletRefil(string TreasuryCode, short ClearBalance)
        {
            var _treasuryCode = new NpgsqlParameter("@_treasury_code", NpgsqlTypes.NpgsqlDbType.Varchar);
            var _clearBalance = new NpgsqlParameter("@_clear_balance", NpgsqlTypes.NpgsqlDbType.Smallint);
            _treasuryCode.Value = TreasuryCode;
            _clearBalance.Value = ClearBalance;

            var _is_done_out = new NpgsqlParameter("@_is_done_out", NpgsqlTypes.NpgsqlDbType.Boolean);
            _is_done_out.Direction = ParameterDirection.InputOutput;
            _is_done_out.Value = false;

            var parameters = new[] { _treasuryCode, _clearBalance, _is_done_out };
            var commandText = "CALL master.wallet_refill(@_treasury_code, @_clear_balance, @_is_done_out)";
            await _context.Database.ExecuteSqlRawAsync(commandText, parameters);
            return (bool)_is_done_out.Value;
        }
    }
}
