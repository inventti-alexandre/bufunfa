using JNogueira.Bufunfa.Api;
using JNogueira.Bufunfa.Api.Swagger;
using JNogueira.Bufunfa.Api.Swagger.Exemplos;
using JNogueira.Bufunfa.Dominio;
using JNogueira.Bufunfa.Dominio.Comandos.Entrada;
using JNogueira.Bufunfa.Dominio.Comandos.Saida;
using JNogueira.Bufunfa.Dominio.Entidades;
using JNogueira.Bufunfa.Dominio.Interfaces.Comandos;
using JNogueira.Bufunfa.Dominio.Interfaces.Servicos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Examples;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Principal;

namespace Bufunfa.Api.Controllers
{
    [Produces("application/json")]
    [SwaggerResponse((int)HttpStatusCode.InternalServerError, typeof(Response), "Erro não tratado encontrado. (Internal Server Error)")]
    [SwaggerResponseExample((int)HttpStatusCode.InternalServerError, typeof(InternalServerErrorApiResponse))]
    [SwaggerResponse((int)HttpStatusCode.NotFound, typeof(Response), "Endereço não encontrado. (Not found)")]
    [SwaggerResponseExample((int)HttpStatusCode.NotFound, typeof(NotFoundApiResponse))]
    public class UsuarioController : Controller
    {
        private readonly IUsuarioServico _usuarioServico;

        public UsuarioController(IUsuarioServico usuarioServico)
        {
            _usuarioServico = usuarioServico;
        }

        /// <summary>
        /// Rota para autenticação do usuário. Caso a autenticação ocorra com sucesso, um token JWT é gerado e retornado.
        /// </summary>
        /// <param name="email">E-mail do usuário</param>
        /// <param name="senha">Senha do usuário</param>
        [AllowAnonymous]
        [HttpPost]
        [Route("v1/usuarios/autenticar")]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(Response), "Caso o usuário seja autenticado com sucesso, o token JWT é retornado.")]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(AutenticarUsuarioApiResponse))]
        public ISaida Autenticar(string email, string senha, [FromServices] JwtTokenConfig tokenConfig /*FromServices: resolvidos via mecanismo de injeção de dependências do ASP.NET Core*/)
        {
            var autenticarComando = new AutenticarUsuarioEntrada { Email = email, Senha = senha };

            var comandoSaida = _usuarioServico.Autenticar(autenticarComando);

            if (!comandoSaida.Sucesso)
                return comandoSaida;

            var usuario = (Usuario)comandoSaida.Retorno;

            var dataCriacaoToken = DateTime.Now;
            var dataExpiracaoToken = dataCriacaoToken + TimeSpan.FromSeconds(tokenConfig.ExpiracaoEmSegundos);

            return CriarResponseTokenJwt(usuario, dataCriacaoToken, dataExpiracaoToken, tokenConfig);
        }

        /// <summary>
        /// Obtém as informações do usuário, a partir do seu e-mail
        /// </summary>
        /// <param name="email">E-mail do usuário.</param>
        [Authorize(PermissaoAcesso.ConsultarUsuario)]
        [HttpGet]
        [Route("v1/usuarios/obter-por-email/{email}")]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(Response), "Informações do usuário encontrado.")]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(ObterUsuarioPorEmailApiResponse))]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized, typeof(Response), "Acesso negado. Token de autenticação não encontrado. (Unauthorized)")]
        [SwaggerResponseExample((int)HttpStatusCode.Unauthorized, typeof(UnauthorizedApiResponse))]
        [SwaggerResponse((int)HttpStatusCode.Forbidden, typeof(Response), "Acesso negado. Sem permissão de acesso a funcionalidade. (Forbidden)")]
        [SwaggerResponseExample((int)HttpStatusCode.Forbidden, typeof(ForbiddenApiResponse))]
        public ISaida ObterPorEmail(string email)
        {
            return _usuarioServico.ObterUsuarioPorEmail(email);
        }

        private ISaida CriarResponseTokenJwt(Usuario usuario, DateTime dataCriacaoToken, DateTime dataExpiracaoToken, JwtTokenConfig tokenConfig)
        {
            var identity = new ClaimsIdentity(
                    new GenericIdentity(usuario.Nome),
                    // Geração de claims. No contexto desse sistema, claims não precisaram ser criadas.
                    new[] {
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                        new Claim(JwtRegisteredClaimNames.UniqueName, usuario.Email)
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

            return new Saida(true, new[] { "Usuário autenticado com sucesso." }, new
            {
                DataCriacaoToken = dataCriacaoToken,
                DataExpiracaoToken = dataExpiracaoToken,
                Token = jwtToken,
            });
        }
    }
}