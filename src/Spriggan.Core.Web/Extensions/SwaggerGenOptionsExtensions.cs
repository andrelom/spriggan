using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Spriggan.Core.Web.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Spriggan.Core.Web.Extensions;

internal static class SwaggerGenOptionsExtensions
{
    public static SwaggerGenOptions LowercaseDocuments(this SwaggerGenOptions options)
    {
        options.DocumentFilter<LowerCaseDocumentFilter>();

        return options;
    }

    public static SwaggerGenOptions AspNetCoreVersioning(this SwaggerGenOptions options)
    {
        // SchemaId already used for different type.
        options.CustomSchemaIds(type => type.ToString());

        // Avoid Swagger generation error due to same method name in different versions.
        options.ResolveConflictingActions(descriptions => descriptions.First());

        return options;
    }

    public static SwaggerGenOptions AddSecurity(this SwaggerGenOptions options)
    {
        options.AddSecurityDefinition("Bearer", new()
        {
            Type = SecuritySchemeType.ApiKey,
            In = ParameterLocation.Header,
            Name = "Authorization",
            Scheme = "Bearer",
            BearerFormat = "JWT",
            Description = "Enter 'Bearer' [space] and your valid token."
        });

        options.AddSecurityRequirement(new()
        {
            {
                new() { Reference = new() { Type = ReferenceType.SecurityScheme, Id = "Bearer" } },
                Array.Empty<string>()
            }
        });

        return options;
    }
}
