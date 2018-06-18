using JNogueira.Bufunfa.Dominio.Comandos.Saida;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Generic;

namespace JNogueira.Bufunfa.Api.Attributes
{
    /// <summary>
    /// Filtro que extrai as mensagens do ModelState e coloca no padrão de saida da API.
    /// </summary>
    public class CustomModelStateValidationFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var lstErros = new List<string>();

                foreach (var modelState in context.ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        lstErros.Add(error.Exception != null ? error.Exception.Message : error.ErrorMessage);
                    }
                }

                context.Result = new OkObjectResult(new Saida(false, lstErros, null));
            }
        }
    }
}
