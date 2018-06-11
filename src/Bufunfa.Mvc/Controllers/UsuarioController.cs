using JNogueira.Bufunfa.Dominio.Comandos.Entrada;
using JNogueira.Bufunfa.Dominio.Comandos.Saida;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;

namespace Bufunfa.Mvc.Controllers
{
    [Authorize]
    public class UsuarioController : Controller
    {
        [AllowAnonymous]
        [HttpGet]
        [Route("login")]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public IActionResult Login(AutenticarUsuarioEntrada autenticarComando)
        {
            var client = new RestClient("http://localhost:5000/v1/usuarios/autenticar");
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("undefined", JsonConvert.SerializeObject(autenticarComando), ParameterType.RequestBody);
            var response = client.Execute<ComandoSaida>(request);

            return new EmptyResult();
        }
    }
}
