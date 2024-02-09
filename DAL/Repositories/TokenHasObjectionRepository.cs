using CTS_BE.DAL.Entities;
using CTS_BE.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Data;

namespace CTS_BE.DAL
{
   public class TokenHasObjectionRepository : Repository<TokenHasObjection, CTSDBContext>, ITokenHasObjectionsRepository
   {
       protected readonly CTSDBContext _cTSDBContext;
       public TokenHasObjectionRepository(CTSDBContext context) : base(context)
       {
            _cTSDBContext = context;
       }
        public async Task<bool> BillCheck(long tokenId,string referenceNo,string billObjections,string overruledObjections,  long userId, int ownType)
        {
            var _tokenId = new NpgsqlParameter("@in_token_id",NpgsqlTypes.NpgsqlDbType.Bigint);
            var _referenceNo = new NpgsqlParameter("@in_reference_no", NpgsqlTypes.NpgsqlDbType.Varchar);
            var _billObjections = new NpgsqlParameter("@in_bill_objections",NpgsqlTypes.NpgsqlDbType.Jsonb);
            var _overruledObjections = new NpgsqlParameter("@in_overruled_objections", NpgsqlTypes.NpgsqlDbType.Jsonb);
            var _userId = new NpgsqlParameter("@in_user_id", NpgsqlTypes.NpgsqlDbType.Bigint);
            var _ownType = new NpgsqlParameter("@in_own_type", NpgsqlTypes.NpgsqlDbType.Smallint);
            var _outputParameter = new NpgsqlParameter("@is_done_out", NpgsqlTypes.NpgsqlDbType.Smallint);
            _tokenId.Value = tokenId;
            _referenceNo.Value = referenceNo;
            _billObjections.Value = billObjections;
            _overruledObjections.Value = overruledObjections;
            _userId.Value = userId;
            _ownType.Value = ownType;
            _outputParameter.Direction = ParameterDirection.InputOutput;
            _outputParameter.Value = 0;
            var parameters = new[] { _tokenId, _referenceNo , _billObjections,_overruledObjections, _userId, _ownType, _outputParameter };
            var commandText = "call cts.bill_Check(@in_token_id, @in_reference_no, @in_bill_objections,@in_overruled_objections,@in_user_id,@in_own_type,@is_done_out)";
            await _cTSDBContext.Database.ExecuteSqlRawAsync(commandText,parameters);
            int isDone = (Int16)_outputParameter.Value;
            return (isDone==0)?false:true;
        }
   }
}