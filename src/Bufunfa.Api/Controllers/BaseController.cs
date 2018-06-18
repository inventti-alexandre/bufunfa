using JNogueira.Bufunfa.Dominio.Comandos.Saida;
using JNogueira.Bufunfa.Dominio.Interfaces.Comandos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JNogueira.Bufunfa.Api.Controllers
{
    [Produces("application/json")]
    public abstract class BaseController : Controller
    {
        /// <summary>
        /// Obtém do token JWT, o ID do usuário
        /// </summary>
        public int ObterIdUsuarioClaim() => Convert.ToInt32(User.Claims.First(x => x.Type == "IdUsuario").Value);

        public ISaida RetornarPorModelStateInvalido()
        {
            var lstMensagens = new List<string>();

            foreach (var item in ModelState)
            {
                if (item.Value.Errors.FirstOrDefault()?.Exception != null)
                {
                    lstMensagens.Add($"{item.Value.Errors.First().Exception.Message} ({item.Key})");
                }
                else
                {
                    lstMensagens.Add($"{item.Value.Errors.FirstOrDefault()?.ErrorMessage} ({item.Key})");
                }
            }

            return new Saida(false, lstMensagens, null);
        }
    }
}
