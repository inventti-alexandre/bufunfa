using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;

namespace JNogueira.Bufunfa.Api.Middlewares
{
    /// <summary>
    /// Middleware para fazer o handler de erros HTTP ou de exceptions e padronizar o retorno.
    /// </summary>
    public class CustomExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);

                switch (context.Response.StatusCode)
                {
                    case (int)HttpStatusCode.Unauthorized:
                        {
                            context.Response.ContentType = "application/json";
                            await context.Response.WriteAsync(JsonConvert.SerializeObject(new UnauthorizedApiResponse()));
                            break;
                        }
                    case (int)HttpStatusCode.Forbidden:
                        {
                            context.Response.ContentType = "application/json";
                            await context.Response.WriteAsync(JsonConvert.SerializeObject(new ForbiddenApiResponse()));
                            break;
                        }
                    case (int)HttpStatusCode.NotFound:
                        {
                            context.Response.ContentType = "application/json";
                            await context.Response.WriteAsync(JsonConvert.SerializeObject(new NotFoundApiResponse(context.Request.Path)));
                            break;
                        }
                }
            }
            catch (Exception exception)
            {
                await HandleExceptionAsync(context, exception);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            return context.Response.WriteAsync(JsonConvert.SerializeObject(new InternalServerErrorApiResponse(exception)));
        }
    }

    public static class CustomExceptionHandlerMiddlewareExtension
    {
        public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder builder) => builder.UseMiddleware(typeof(CustomExceptionHandlerMiddleware));
    }
}
