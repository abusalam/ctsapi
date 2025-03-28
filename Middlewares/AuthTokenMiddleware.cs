using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using CTS_BE.Enum;
using CTS_BE.Helper;
using CTS_BE.Model.Claims;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.IdentityModel.Tokens;

namespace CTS_BE.Middlewares
{
    public class AuthTokenMiddleware(
        RequestDelegate next,
        ILogger<AuthTokenMiddleware> logger,
        IConfiguration Configuration
    )
    {
        private readonly RequestDelegate _next = next;
        private readonly IConfiguration _Configuration = Configuration;
        private readonly ILogger<AuthTokenMiddleware> _logger = logger;

        private Dictionary<string, string> _allowedTokens = [];

        private void RemoveExpiredTokens(int maxRefreshTokenValidityInSecs = 3600)
        {
            JwtSecurityTokenHandler tokenHandler = new();

            _allowedTokens
                .Where(c =>
                {
                    return long.Parse(
                            tokenHandler
                                .ReadJwtToken(c.Value)
                                .Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Exp)
                                ?.Value ?? "0"
                        )
                        <= (
                            DateTimeOffset.UtcNow.ToUnixTimeSeconds()
                            - maxRefreshTokenValidityInSecs
                        );
                })
                .Select(c => c.Key)
                .ToList()
                .ForEach(item =>
                {
                    _allowedTokens.Remove(item);
                    Console.WriteLine($"Removing Token: {item}");
                });
        }

        private static async Task Prepare401Response(string message, HttpContext context)
        {
            JsonAPIResponse<bool> response = new()
            {
                Message = message,
                ApiResponseStatus = APIResponseStatus.Error,
                Result = false,
            };
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "application/json; charset=utf-8";
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }

        public async Task Invoke(HttpContext context)
        {
            var response = new JsonAPIResponse<bool>();
            string tokenId = "--";

            try
            {
                string? token =
                    context.Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last() ?? "";

                string secretKey = _Configuration?["Auth:SecretKey"] ?? "";
                string jwtIssuer = _Configuration?["Auth:Issuer"] ?? "";
                string jwtAudience = _Configuration?["Auth:Audience"] ?? "";

                if (secretKey == "" || jwtIssuer == "" || jwtAudience == "")
                {
                    response.ApiResponseStatus = APIResponseStatus.Error;
                    response.Message =
                        "Missing required config: "
                        + (secretKey == "" ? "Auth:SecretKey, " : "")
                        + (jwtIssuer == "" ? "Auth:Issuer, " : "")
                        + (jwtAudience == "" ? "Auth:Audience, " : "").TrimEnd(',', ' ')
                        + $" in appsettings for {Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json";
                    response.Result = false;

                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                    return;
                }

                if (!string.IsNullOrWhiteSpace(token))
                {
                    TokenValidationParameters validationParameters = new()
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(secretKey)
                        ),
                        ValidateIssuer = true,
                        ValidIssuer = jwtIssuer,
                        ValidateAudience = true,
                        ValidAudience = jwtAudience,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,
                    };

                    JwtSecurityTokenHandler tokenHandler = new();

                    ClaimsPrincipal principal = tokenHandler.ValidateToken(
                        token,
                        validationParameters,
                        out SecurityToken validatedToken
                    );

                    bool isAuthenticated = principal.Identity?.IsAuthenticated ?? false;
                    AuthClaimModel jwtAuthClaimModel = new() { claims = [.. principal.Claims] };

                    if (isAuthenticated)
                    {
                        tokenId =
                            principal
                                .Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)
                                ?.Value ?? "0";
                        if (tokenId == "0")
                        {
                            await Prepare401Response("TokenID is not valid.", context);
                            return;
                        }

                        string prevTokenId =
                            principal.Claims.FirstOrDefault(c => c.Type == "pti")?.Value ?? "0";
                        if (prevTokenId == "0")
                        {
                            await Prepare401Response("Previous TokenID is not valid.", context);
                            return;
                        }

                        string tokenType =
                            principal
                                .Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Typ)
                                ?.Value ?? "0";
                        if (tokenType == "ref")
                        {
                            await Prepare401Response(
                                $"Use of refresh token ID: {tokenId} is not allowed to access APIs.",
                                context
                            );
                            return;
                        }

                        bool isLoginRequest =
                            context.Request.Path.Value?.ToLower().Contains("login") == true;
                        if (isLoginRequest && !_allowedTokens.ContainsKey(tokenId))
                        {
                            _allowedTokens[tokenId] = token;
                            Console.WriteLine($"Using new token (Login): {prevTokenId}:{tokenId}");
                        }

                        if (_allowedTokens.ContainsKey(prevTokenId))
                        {
                            _allowedTokens.Remove(prevTokenId);
                            _allowedTokens[tokenId] = token;
                        }

                        if (!_allowedTokens.ContainsKey(tokenId))
                        {
                            Console.WriteLine($"Use of revoked Token: {tokenId} blocked.");
                            await Prepare401Response(
                                $"Unauthorized use of revoked token ID: {tokenId}.",
                                context
                            );
                            return;
                        }

                        if (context.Request.Path.Value?.ToLower().Contains("logout") == true)
                        {
                            _allowedTokens.Remove(tokenId);
                            Console.WriteLine($"Revoked token (Logout): {prevTokenId}:{tokenId}");
                        }

                        Console.WriteLine("API-URL: " + context.Request.GetDisplayUrl());
                        _allowedTokens
                            .Select(i => $"{i.Key}: ...{i.Value[^8..]}")
                            .ToList()
                            .ForEach(Console.WriteLine);
                        Console.WriteLine("Allowed Tokens: " + _allowedTokens.Count);

                        string expiration =
                            principal
                                .Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Exp)
                                ?.Value ?? "0";
                        if (expiration != "0" && long.TryParse(expiration, out long expUnix))
                        {
                            long remainingTime =
                                expUnix - DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                            context.Response.Headers.Append(
                                "Remaining-Time",
                                remainingTime.ToString()
                            );
                            context.Response.Headers.Append(
                                "Access-Control-Expose-Headers",
                                "Remaining-Time"
                            );
                        }

                        context.Items["userclaimmodel"] = jwtAuthClaimModel;
                        RemoveExpiredTokens();
                        await _next(context);
                    }
                    else
                    {
                        await Prepare401Response("Token is not authenticated.", context);
                    }
                }
                else
                {
                    // No token - continue without authorization if allowed
                    await _next(context);
                }
            }
            catch (SecurityTokenNotYetValidException ex)
            {
                await Prepare401Response(
                    $"Token not yet valid. TokenId: {tokenId}. {ex.Message}",
                    context
                );
            }
            catch (SecurityTokenExpiredException ex)
            {
                await Prepare401Response(
                    $"Token expired. TokenId: {tokenId}. {ex.Message}",
                    context
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                response.ApiResponseStatus = APIResponseStatus.Error;
                response.Message =
                    $"Server Error TokenId: {tokenId}, {ex.InnerException} {ex.Message}";
                response.Result = false;

                context.Response.StatusCode = StatusCodes.Status412PreconditionFailed;
                context.Response.ContentType = "application/json; charset=utf-8";
                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
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
