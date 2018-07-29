using JNogueira.Bufunfa.Api;
using JNogueira.Bufunfa.Api.Swagger;
using JNogueira.Bufunfa.Api.Swagger.Exemplos;
using JNogueira.Bufunfa.Dominio.Comandos.Entrada;
using JNogueira.Bufunfa.Dominio.Comandos.Saida;
using JNogueira.Bufunfa.Dominio.Interfaces.Comandos;
using JNogueira.Bufunfa.Dominio.Interfaces.Servicos;
using JNogueira.Bufunfa.Dominio.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace Bufunfa.Api.Controllers
{
    [Produces("application/json")]
    [SwaggerResponse((int)HttpStatusCode.InternalServerError, "Erro não tratado encontrado. (Internal Server Error)", typeof(Response))]
    [SwaggerResponseExample((int)HttpStatusCode.InternalServerError, typeof(InternalServerErrorApiResponse))]
    [SwaggerResponse((int)HttpStatusCode.NotFound, "Endereço não encontrado. (Not found)", typeof(Response))]
    [SwaggerResponseExample((int)HttpStatusCode.NotFound, typeof(NotFoundApiResponse))]
    public class UsuarioController : Controller
    {
        private readonly IUsuarioServico _usuarioServico;

        public UsuarioController(IUsuarioServico usuarioServico)
        {
            _usuarioServico = usuarioServico;
        }

        /// <summary>
        /// Realiza a autenticação do usuário, a partir do e-mail e senha informados.
        /// </summary>
        [AllowAnonymous]
        [HttpPost]
        [Route("v1/usuarios/autenticar")]
        [SwaggerOperation(Description = "Caso a autenticação ocorra com sucesso, um token JWT é gerado e retornado.")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Caso o usuário seja autenticado com sucesso, o token JWT é retornado.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(AutenticarUsuarioResponseExemplo))]
        public async Task<ISaida> Autenticar(
            [SwaggerParameter("E-mail do usuário.", Required = true)] string email,
            [SwaggerParameter("Senha do usuário.", Required = true)] string senha,
            [FromServices] JwtTokenConfig tokenConfig /*FromServices: resolvidos via mecanismo de injeção de dependências do ASP.NET Core*/)
        {
            var autenticarComando = new AutenticarUsuarioEntrada(email, senha);

            var comandoSaida = await _usuarioServico.Autenticar(autenticarComando);

            if (!comandoSaida.Sucesso)
                return comandoSaida;

            var usuario = (UsuarioSaida)comandoSaida.Retorno;

            var dataCriacaoToken = DateTime.Now;
            var dataExpiracaoToken = dataCriacaoToken + TimeSpan.FromSeconds(tokenConfig.ExpiracaoEmSegundos);

            return CriarResponseTokenJwt(usuario, dataCriacaoToken, dataExpiracaoToken, tokenConfig);
        }

        private ISaida CriarResponseTokenJwt(UsuarioSaida usuario, DateTime dataCriacaoToken, DateTime dataExpiracaoToken, JwtTokenConfig tokenConfig)
        {
            var identity = new ClaimsIdentity(
                    new GenericIdentity(usuario.Nome),
                    // Geração de claims. No contexto desse sistema, claims não precisaram ser criadas.
                    new[] {
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                        new Claim(JwtRegisteredClaimNames.UniqueName, usuario.Email),
                        new Claim("IdUsuario", usuario.Id.ToString())
                    }
                    // Adiciona as perissões de acesso do usuário
                    .Union(usuario.PermissoesAcesso.Select(x => new Claim(x, x)))
                );

            var jwtHandler = new JwtSecurityTokenHandler();

            var securityToken = jwtHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = tokenConfig.Issuer,
                Audience = tokenConfig.Audience,
                SigningCredentials = tokenConfig.SigningCredentials,
                Subject = identity,
                NotBefore = dataCriacaoToken,
                Expires = dataExpiracaoToken
            });

            // Cria o token JWT em formato de string
            var jwtToken = jwtHandler.WriteToken(securityToken);

            return new Saida(true, new[] { UsuarioMensagem.Usuario_Autenticado_Com_Sucesso }, new
            {
                DataCriacaoToken = dataCriacaoToken,
                DataExpiracaoToken = dataExpiracaoToken,
                Token = jwtToken,
            });
        }
    }
}