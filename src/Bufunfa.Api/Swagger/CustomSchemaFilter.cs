using JNogueira.Bufunfa.Api.Swagger.Responses;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace JNogueira.Bufunfa.Api.Swagger
{
    public class CustomSchemaFilter : ISchemaFilter
    {
        public void Apply(Schema model, SchemaFilterContext context)
        {
            if (context.SystemType == typeof(ResponseInternalServerError))
                model.Example = new ResponseInternalServerError();
        }
    }
}
