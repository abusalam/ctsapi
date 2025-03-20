using System.Reflection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CTS_BE.Filters
{
    public class SwaggerExcludeFilter : ISchemaFilter //, IDocumentFilter
    {
        private static readonly HashSet<string> ExcludedKeys = [];

        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (schema.Properties != null)
            {
                var excludedProperties = context
                    .Type.GetProperties()
                    .Where(t => t.GetCustomAttribute<SwaggerExcludeAttribute>() != null);

                foreach (var excludedProperty in excludedProperties)
                {
                    if (excludedProperty.ReflectedType?.Name.Contains("Entry") ?? false)
                    {
                        // Console.WriteLine(
                        //     excludedProperty.ReflectedType?.Name + " => " + excludedProperty.Name
                        // );
                        var propertyToRemove = schema.Properties.Keys.SingleOrDefault(x =>
                            string.Equals(
                                x,
                                excludedProperty.Name,
                                StringComparison.OrdinalIgnoreCase
                            )
                        );
                        schema.Properties.Remove(propertyToRemove);
                    }
                }
            }
        }

        // public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        // {
        //     foreach (var key in swaggerDoc.Components.Schemas.Keys)
        //     {
        //         if (key.Contains("Entry"))
        //         {
        //             Console.WriteLine(key);
        //         }
        //     }
        // }
    }
}
