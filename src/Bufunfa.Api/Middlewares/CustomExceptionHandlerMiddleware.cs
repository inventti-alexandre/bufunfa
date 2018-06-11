using JNogueira.Bufunfa.Dominio.Comandos.Saida;
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
                            await context.Response.WriteAsync(JsonConvert.SerializeObject(new ComandoSaida(false, new[] { "Erro 401: Acesso negado. Certifique-se que você foi autenticado." }, null)));
                            break;
                        }
                    case (int)HttpStatusCode.Forbidden:
                        {
                            context.Response.ContentType = "application/json";
                            await context.Response.WriteAsync(JsonConvert.SerializeObject(new ComandoSaida(false, new[] { "Erro 403: Acesso negado. Você não tem permissão de acesso para essa funcionalidade." }, null)));
                            break;
                        }
                    case (int)HttpStatusCode.NotFound:
                        {
                            context.Response.ContentType = "application/json";
                            await context.Response.WriteAsync(JsonConvert.SerializeObject(new ComandoSaida(false, new[] { $"Erro 404: O endereço \"{context.Request.Path}\" não foi encontrado." }, null)));
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
            var comandoSaida = new ComandoSaida(false, new[] { exception.Message }, new
            {
                Exception = exception.Message,
                BaseException = exception.GetBaseException().Message,
                Source = exception.Source
            });

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            return context.Response.WriteAsync(JsonConvert.SerializeObject(comandoSaida));
        }
    }

    public static class CustomExceptionHandlerMiddlewareExtension
    {
        public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder builder) => builder.UseMiddleware(typeof(CustomExceptionHandlerMiddleware));
    }
}
