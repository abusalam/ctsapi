using System.Text.Json;
using CTS_BE.DAL.Interfaces.Pension;
using CTS_BE.Enum;
using CTS_BE.Helper;

namespace CTS_BE.Middlewares
{
    public class FinancialYearMiddleware(
        RequestDelegate next,
        ILogger<FinancialYearMiddleware> logger
    )
    {
        private readonly RequestDelegate _next = next;
        private readonly ILogger<FinancialYearMiddleware> _logger = logger;

        public async Task InvokeAsync(HttpContext context)
        {
            // Check if already present, early exit for performance
            if (context.Items.ContainsKey("FinancialYear"))
            {
                await _next(context);
                return;
            }

            try
            {
                using IServiceScope? scope = context.RequestServices.CreateScope();
                IFinancialYearRepository? financialYearRepository =
                    scope.ServiceProvider.GetRequiredService<IFinancialYearRepository>();

                short financialYear = await financialYearRepository.GetCurrentFinancialYearAsync();

                if (financialYear != 0)
                {
                    context.Items["FinancialYear"] = financialYear;
                    await _next(context);
                }
                else
                {
                    _logger.LogWarning("Unable to retrieve current financial year from database.");
                    await WriteJsonResponseAsync(
                        context,
                        new JsonAPIResponse<object> // Use object for empty result
                        {
                            ApiResponseStatus = APIResponseStatus.Error,
                            Message = "Unable to retrieve current financial year from database.",
                        },
                        StatusCodes.Status412PreconditionFailed
                    );
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "An error occurred while retrieving the current financial year."
                );
                await WriteJsonResponseAsync(
                    context,
                    new JsonAPIResponse<object>
                    {
                        ApiResponseStatus = APIResponseStatus.Error,
                        Message =
                            $"An error occurred while retrieving the current financial year. {ex.Message} {ex.StackTrace}",
                    },
                    StatusCodes.Status412PreconditionFailed
                );
            }
        }

        private static async Task WriteJsonResponseAsync<T>(
            HttpContext context,
            JsonAPIResponse<T> response,
            int statusCode
        )
        {
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            string? json = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(json);
        }
    }

    public static class FinancialYearMiddlewareExtensions
    {
        public static IApplicationBuilder UseFinancialYear(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<FinancialYearMiddleware>();
        }
    }
}
