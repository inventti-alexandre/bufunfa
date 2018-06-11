using JNogueira.Bufunfa.Api.Seguranca;
using JNogueira.Bufunfa.Dominio;
using JNogueira.Bufunfa.Dominio.Comandos.Entrada;
using JNogueira.Bufunfa.Dominio.Comandos.Saida;
using JNogueira.Bufunfa.Dominio.Entidades;
using JNogueira.Bufunfa.Dominio.Interfaces.Comandos;
using JNogueira.Bufunfa.Dominio.Interfaces.Servicos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;

namespace Bufunfa.Api.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly IUsuarioServico _usuarioServico;

        public UsuarioController(IUsuarioServico usuarioServico)
        {
            _usuarioServico = usuarioServico;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("v1/usuarios/autenticar")]
        public IComandoSaida Autenticar([FromBody] AutenticarUsuarioEntrada autenticarComando /*FromBody: resolvidos a partir do body do request.*/,
                                        [FromServices] JwtTokenConfig tokenConfig /*FromServices: resolvidos via mecanismo de injeção de dependências do ASP.NET Core*/)
        {
            var comandoSaida = _usuarioServico.Autenticar(autenticarComando);

            if (!comandoSaida.Sucesso)
                return comandoSaida;

            var usuario = (Usuario)comandoSaida.Retorno;

            var dataCriacaoToken = DateTime.Now;
            var dataExpiracaoToken = dataCriacaoToken + TimeSpan.FromSeconds(tokenConfig.ExpiracaoEmSegundos);

            return CriarResponseTokenJwt(usuario, dataCriacaoToken, dataExpiracaoToken, tokenConfig);
        }

        [Authorize(PermissaoAcesso.ConsultarUsuario)]
        [HttpGet]
        [Route("v1/usuarios/obter-por-email/{email}")]
        public IComandoSaida ObterUsuarioPorEmail(string email)
        {
            return _usuarioServico.ObterUsuarioPorEmail(email);
        }

        private IComandoSaida CriarResponseTokenJwt(Usuario usuario, DateTime dataCriacaoToken, DateTime dataExpiracaoToken, JwtTokenConfig tokenConfig)
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

            return new ComandoSaida(true, new[] { "Usuário autenticado com sucesso." }, new
            {
                DataCriacaoToken = dataCriacaoToken,
                DataExpiracaoToken = dataExpiracaoToken,
                Token = jwtToken,
            });
        }
    }
}