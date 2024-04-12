using CTS_BE.DAL.Entities;
using CTS_BE.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Data;

namespace CTS_BE.DAL.Repositories
{
    public class TokenRepository : Repository<Token, CTSDBContext>, ITokenRepository
    {
        protected readonly CTSDBContext _context;
        public TokenRepository(CTSDBContext context) : base(context)
        {
            _context = context;
        }
        public async Task<int> GenarateToken(long billid, long userid,string remarks,DateOnly phybilldate)
        {

            var _billid = new NpgsqlParameter("@in_bill_id", NpgsqlTypes.NpgsqlDbType.Bigint);
            var _userid = new NpgsqlParameter("@in_logged_in_user_id", NpgsqlTypes.NpgsqlDbType.Bigint);
            var _remarks = new NpgsqlParameter("@in_remarks", NpgsqlTypes.NpgsqlDbType.Varchar);
            var _phybilldate = new NpgsqlParameter("@in_phybilldate", NpgsqlTypes.NpgsqlDbType.Date);
            var _outputParameter = new NpgsqlParameter("@token_number_out", NpgsqlTypes.NpgsqlDbType.Integer);
            _billid.Value = billid;
            _userid.Value = userid;
            _remarks.Value = remarks;
            _phybilldate.Value = phybilldate;
            _outputParameter.Direction = ParameterDirection.InputOutput;

            // Set an initial value for the output parameter
            _outputParameter.Value = 0; // Replace with an appropriate initial value if needed

            var parameters = new[] { _billid, _userid, _remarks,_phybilldate ,_outputParameter };
            var commandText = "call cts.token_genaration(@in_bill_id, @in_logged_in_user_id,@in_remarks,@in_phybilldate,@token_number_out)";

            await _context.Database.ExecuteSqlRawAsync(commandText, parameters);

            int insertedId = (int)_outputParameter.Value;
            return insertedId;
        }
        public async Task<bool> GenerateReturnMemo(long tokenId,string referenceNo,long userId,int ownType)
        {
            var _tokenId = new NpgsqlParameter("@in_token_id", NpgsqlTypes.NpgsqlDbType.Bigint);
            var _referenceNo = new NpgsqlParameter("@in_reference_no", NpgsqlTypes.NpgsqlDbType.Varchar);
            var _userId = new NpgsqlParameter("@in_user_id", NpgsqlTypes.NpgsqlDbType.Bigint);
            var _ownType= new NpgsqlParameter("@in_own_type", NpgsqlTypes.NpgsqlDbType.Smallint);
            var _outputParameter = new NpgsqlParameter("@is_done_out", NpgsqlTypes.NpgsqlDbType.Smallint);
            _tokenId.Value = tokenId;
            _referenceNo.Value = referenceNo;
            _userId.Value = userId;
            _ownType.Value = ownType;
            _outputParameter.Direction = ParameterDirection.InputOutput;
            _outputParameter.Value = 0;
            var parameters = new[] { _tokenId, _referenceNo, _userId, _ownType,_outputParameter };
            var commandText = "CALL cts.generate_return_memo(@in_token_id, @in_reference_no, @in_user_id, @in_own_type, @is_done_out)";
            await _context.Database.ExecuteSqlRawAsync(commandText,parameters);
            int isDone = (Int16)_outputParameter.Value;
            return (isDone == 0) ? false : true;
        }
        public async Task<bool> PaymandateShortList(long loggedinUserId,string paymentData)
        {
            var _userId = new NpgsqlParameter("@in_loggedin_user_id",NpgsqlTypes.NpgsqlDbType.Bigint);
            var _paymandate_paylod = new NpgsqlParameter("@in_paymandate_paylod",NpgsqlTypes.NpgsqlDbType.Jsonb);
            var _outputParameter = new NpgsqlParameter("@is_done_out", NpgsqlTypes.NpgsqlDbType.Smallint);
            _userId.Value = loggedinUserId;
            _paymandate_paylod.Value = paymentData;
            _outputParameter.Direction = ParameterDirection.InputOutput;
            _outputParameter.Value = 0;
            var parameters = new[] {_userId, _paymandate_paylod,_outputParameter};
            var commandText = "CALL  cts.paymandate(@in_loggedin_user_id,@in_paymandate_paylod,@is_done_out)";
            await _context.Database.ExecuteSqlRawAsync(commandText,parameters);
            int isDone = (Int16)_outputParameter.Value;
            return (isDone == 0) ? false : true;
        }
    }
}