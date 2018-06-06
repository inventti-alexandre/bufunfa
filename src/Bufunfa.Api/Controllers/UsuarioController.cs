using JNogueira.Bufunfa.Api;
using JNogueira.Bufunfa.Dominio.Comandos.Entrada.Usuario;
using JNogueira.Bufunfa.Dominio.Entidades;
using JNogueira.Bufunfa.Dominio.Interfaces.Comandos;
using JNogueira.Bufunfa.Dominio.Interfaces.Servicos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
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
        public object Autenticar([FromBody] AutenticarUsuarioComando autenticarComando, [FromServices]TokenParametros tokenParam)
        {
            var comandoSaida = _usuarioServico.Autenticar(autenticarComando);

            if (!comandoSaida.Sucesso)
                return comandoSaida;

            var usuario = (Usuario)comandoSaida.Retorno;

            var identity = new ClaimsIdentity(
                    new GenericIdentity(usuario.Email),
                    new[] {
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                        new Claim(JwtRegisteredClaimNames.UniqueName, usuario.Email)
                    }
                );

            var dataCriacao = DateTime.Now;
            var dataExpiracao = dataCriacao + TimeSpan.FromSeconds(120);

            var handler = new JwtSecurityTokenHandler();

            SigningCredentials signingCredentials;

            using (var provider = new RSACryptoServiceProvider(2048))
            {
                var key = new RsaSecurityKey(provider.ExportParameters(true));

                signingCredentials = new SigningCredentials(key, SecurityAlgorithms.RsaSha256Signature);
            }

            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = tokenParam.Issuer,
                Audience = tokenParam.Audience,
                SigningCredentials = signingCredentials,
                Subject = identity,
                NotBefore = dataCriacao,
                Expires = dataExpiracao
            });

            var token = handler.WriteToken(securityToken);

            return new
            {
                authenticated = true,
                created = dataCriacao.ToString("yyyy-MM-dd HH:mm:ss"),
                expiration = dataExpiracao.ToString("yyyy-MM-dd HH:mm:ss"),
                accessToken = token
            };
        }
    }
}