using CTS_BE.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

namespace CTS_BE.Seeders
{
    public class DatabaseSeeder
    {
        private static readonly ILogger<DatabaseSeeder> _logger =
            NullLogger<DatabaseSeeder>.Instance;

        public static bool Initialize(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
            {
                return false;
            }

            // Get the logger. Use Null Logger if logging is not configured. This avoids null reference exceptions.
            var logger = serviceProvider.GetService<ILogger<DatabaseSeeder>>() ?? _logger;

            var dbContextOptions = serviceProvider.GetService<DbContextOptions<PensionDbContext>>();
            if (dbContextOptions == null)
            {
                logger.LogError("DbContextOptions<PensionDbContext> is not registered.");
                return false;
            }

            using var context = new PensionDbContext(dbContextOptions);

            if (context == null)
            {
                logger.LogError("PensionDbContext could not be created.");
                return false;
            }

            // Get all ISeeder implementations via reflection
            var seeders = typeof(DatabaseSeeder)
                .Assembly.GetTypes()
                .Where(t => typeof(ISeeder).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
                .Select(t =>
                {
                    try
                    {
                        return (ISeeder?)Activator.CreateInstance(t); // Cast to nullable ISeeder
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(
                            ex,
                            "Failed to create instance of seeder type {SeederType}",
                            t.FullName ?? "Unknown Type"
                        ); //Null-coalescing operator for t.FullName
                        return null;
                    }
                })
                .Where(s => s != null)
                .ToList();

            // Execute each seeder
            foreach (var seeder in seeders)
            {
                try
                {
                    seeder!.Seed(context); // Null-forgiving operator, safe because of the Where clause above
                    context.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    logger.LogError(
                        ex,
                        "Error during seeding with {SeederType}",
                        seeder?.GetType().FullName ?? "Unknown Seeder"
                    ); //Null-conditional and null-coalescing operator
                    return false;
                }
            }
            return false;
        }
    }
}
