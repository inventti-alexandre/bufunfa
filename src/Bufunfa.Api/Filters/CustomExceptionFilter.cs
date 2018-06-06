using JNogueira.Bufunfa.Dominio.Comandos.Saida;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Net;

namespace JNogueira.Bufunfa.Api.Filters
{
    public class CustomExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var contextException = context.Exception;
            var baseException = contextException.GetBaseException();

            var httpStatus = HttpStatusCode.InternalServerError;
            ComandoSaida comandoSaida;

            if (contextException.GetType() == typeof(UnauthorizedAccessException))
            {
                httpStatus = HttpStatusCode.Unauthorized;
                comandoSaida = new ComandoSaida(false, new[] { "Acesso negado. Você não posssui permissão de acesso a essa funcionalidade." }, null);
            }
            else
            {
                comandoSaida = new ComandoSaida(false, new[] { baseException.Message }, null);
            }
            
            var response = context.HttpContext.Response;
            response.StatusCode = (int)httpStatus;
            response.ContentType = "application/json";

            context.Result = new JsonResult(comandoSaida);
        }
    }
}
