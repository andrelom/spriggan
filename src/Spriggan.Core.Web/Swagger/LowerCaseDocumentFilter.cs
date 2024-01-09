using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Spriggan.Core.Web.Swagger;

internal sealed class LowerCaseDocumentFilter : IDocumentFilter
{
    public void Apply(OpenApiDocument document, DocumentFilterContext context)
    {
        var paths = new OpenApiPaths();

        var dictionary = document.Paths.ToDictionary(
            entry => entry.Key.ToLower(),
            entry => entry.Value);

        foreach (var (key, value) in dictionary)
        {
            foreach (var param in value.Operations.SelectMany(type => type.Value.Parameters))
            {
                param.Name = param.Name.ToLower();
            }

            paths.Add(key, value);
        }

        document.Paths = paths;
    }
}
