using CTS_BE.Helper.Authentication;

namespace CTS_BE.Middlewares
{

    public class AuthTokenMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _Configuration;
        private readonly ILogger<AuthTokenMiddleware> _logger;
        private readonly ITokenHelper _tokenHelper;
        public AuthTokenMiddleware(RequestDelegate next, ILogger<AuthTokenMiddleware> logger,
            IConfiguration Configuration, ITokenHelper tokenHelper)
        {
            _next = next;
            _Configuration = Configuration;
            _logger = logger;
            _tokenHelper = tokenHelper;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                //var token = "D61E7wRC6U6NLmEPAf8lWmP8ofQc6WrrYA4XWonP6g9Ah211PutS4ehqb/rt9X9BFw2ouSHSpE6i/ooCHVN53Sjegx3VDt7JBqN6CK7blQ84QlTRkM8cfCi2tLVv2xuB4F1QC/W2OsRg/kmHP0kTb/o8N/Ol00qDiPolyVwplAZV5ncl+RAjb8FL27xupAsT0leV4ovxCJU5KVpiMFSP+I6f7Qocgwjm8najDlSqo/CWdE90JPF4zvgzyIsYy8PpFl7gB8TEDBgEKOOr921YHa5MMocldhAjJXRSvIyA6x876vDmzlGf9Jfzqk8ORnaooBVCN5rx9NCu6zf46xlaIyZ4+q0x2Hm/yLYD+5tPglNIV+h8lj+zrmjNY8gGigK+Sd4Rskh2VeTudlDDHaVqd5yENTHrCqtc/nrConfK3LSCfxZYppZqhTszsb25bLsHodtrQnLtp2LHh5wyQLsfc1B0hejbOc6CQ4aEerf0bSmXMIAyuj1KRemiowi7bCczFyLOXEDL7Bsi1hl00/Zm32Qo0gdLXh8ybbQRArFI1CsYzgu25tQO6EDXbCzmOOh1vI764MBYFtqAEhzmQgUty5Ku4V9TuRbfavp8pX9uT0FyOZfeM07aiKzE26KnrcFH6lYl12l+XhT2ilLkTDdCOYyu+M2m8pmzaUJgh24XjFI=";
                if (token != null)
                {
                    //token = AESEncrytDecry.DecryptStringAES(token, _Configuration.GetSection("Auth:AesKey").Value, _Configuration.GetSection("Auth:AesIV").Value);
                    var authClaimModel = _tokenHelper.ValidateAndGetTokenClaims(token, out bool RefreshedAccessTokenRecieved);
                    if (authClaimModel != null)
                    {
                        context.Items["userclaimmodel"] = authClaimModel;
                        if(RefreshedAccessTokenRecieved)
                            context.Items["RefreshedAccessTokenRecieved"] = true;
                        await _next(context);
                    }
                    else
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        _logger.LogError(new Exception("UnAuthenticated"), "UnAuthenticated");
                        await context.Response.WriteAsync("UnAuthenticated");
                    }
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync("{ \"middlewareException\":\""+ex.Message+"\"}");

            }
        }

    }
    public static class AuthTokenMiddlewareExtensions
    {
        public static IApplicationBuilder UseAuthTokenMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthTokenMiddleware>();
        }
    }
}
