//using CTS_BE.DAL.Enums;
using CTS_BE.Model.Claims;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CTS_BE.Helper.Authentication
{
    public interface ITokenHelper
    {
        //public string GenerateToken(UserClaimModel user, TokenType _tokenType, out DateTime ValidTo);
        public SecurityToken ValidateToken(string token, out int LifetimeExpirtedFlag);
        AuthClaimModel ValidateAndGetTokenClaims(string token, out bool RefreshedAccessTokenRecieved);
        public void InvalidateUserLogin(List<Guid> UserIds);
    }
}
