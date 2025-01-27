using CTS_BE.DAL;
using CTS_BE.Helper;
using CTS_BE.Helper.Authentication;
using CTS_BE.Seeders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CTS_BE.Controllers.Pension
{
    public class DatabaseController(
        IClaimService claimService,
        IServiceProvider serviceProvider,
        ILogger<DatabaseController> logger
    ) : ApiBaseController(claimService)
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider;
        private readonly ILogger<DatabaseController> _logger = logger;

        [HttpPost("db/migrate")]
        [Tags("Database Management")]
        [OpenApi]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status412PreconditionFailed)]
        [ProducesDefaultResponseType]
        public IActionResult MigrateDatabase()
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                using var context = scope.ServiceProvider.GetRequiredService<PensionDbContext>();
                // VERY IMPORTANT: Double-check the environment and connection string
                // before executing this in any non-development environment.
                if (!context.Database.IsNpgsql())
                {
                    return BadRequest("Database is not Postgres. Update operation aborted.");
                }

                if (context.Database.GetPendingMigrations().Any())
                {
                    context.Database.Migrate();
                    return Ok("Database migrated successfully.");
                }
                else
                    return Ok("Database is already up to date.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred updating the database.");
                return StatusCode(
                    StatusCodes.Status412PreconditionFailed,
                    "An error occurred updating the database."
                );
            }
        }

        [HttpPut("db/seed")] // Use PUT as it modifies data
        [Tags("Database Management")]
        [OpenApi]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status412PreconditionFailed)]
        [ProducesDefaultResponseType]
        public IActionResult SeedDatabase()
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                using var context = scope.ServiceProvider.GetRequiredService<PensionDbContext>();
                if (context.FinancialYears.Any())
                {
                    return Ok("Database is already seeded");
                }

                if (DatabaseSeeder.Initialize(_serviceProvider))
                {
                    return Ok("Database seeded successfully.");
                }
                else
                {
                    return StatusCode(
                        StatusCodes.Status412PreconditionFailed,
                        "An error occurred seeding the database. Check server log for details"
                    );
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred seeding the DB.");
                return StatusCode(
                    StatusCodes.Status412PreconditionFailed,
                    "An error occurred seeding the database."
                ); // Return 412 Precondition Failed
            }
        }

        [HttpDelete("db/drop")]
        [Tags("Database Management")]
        [OpenApi]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status412PreconditionFailed)]
        [ProducesDefaultResponseType]
#if DEBUG // Only include this in DEBUG builds
        public IActionResult DropDatabase()
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                using var context = scope.ServiceProvider.GetRequiredService<PensionDbContext>();
                // VERY IMPORTANT: Double-check the environment and connection string
                // before executing this in any non-development environment.
                if (!context.Database.IsNpgsql())
                {
                    return BadRequest("Database is not Postgres. Drop operation aborted.");
                }
                if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "Development")
                {
                    return StatusCode(
                        StatusCodes.Status412PreconditionFailed,
                        "Drop operation is only allowed in Development environment."
                    );
                }

                bool dropped = context.Database.EnsureDeleted();
                if (dropped)
                    return Ok("Database dropped successfully.");
                else
                    return Ok("Database does not exists.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred dropping the database.");
                return StatusCode(
                    StatusCodes.Status412PreconditionFailed,
                    "An error occurred dropping the database."
                );
            }
        }
#else
        public IActionResult DropDatabase()
        {
            return NotFound(); // Return 404 Not Found in non-DEBUG builds
        }
#endif
    }
}
