using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace P2PLearningAPI.Extensions
{
    public class SwaggerFileUploadOperationFilter: IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var fileParameters = context.MethodInfo.GetParameters()
                .Where(p => p.ParameterType == typeof(IFormFile));

            foreach (var param in fileParameters)
            {
                operation.RequestBody = new OpenApiRequestBody
                {
                    Content = {
                    ["multipart/form-data"] = new OpenApiMediaType
                    {
                        Schema = new OpenApiSchema
                        {
                            Type = "object",
                            Properties = {
                                [param.Name] = new OpenApiSchema
                                {
                                    Type = "string",
                                    Format = "binary"
                                }
                            },
                            Required = new HashSet<string> { param.Name }
                        }
                    }
                }
                };
            }
        }
    }
}
