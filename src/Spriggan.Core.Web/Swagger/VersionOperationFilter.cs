using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Spriggan.Core.Web.Swagger;

internal sealed class VersionOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var version = operation.Parameters.Single(parameter =>
            parameter.Name.Equals("version", StringComparison.OrdinalIgnoreCase));

        operation.Parameters.Remove(version);
    }
}
