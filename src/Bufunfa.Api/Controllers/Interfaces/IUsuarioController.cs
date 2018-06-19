using JNogueira.Bufunfa.Dominio.Interfaces.Comandos;

namespace JNogueira.Bufunfa.Api.Controllers.Interfaces
{
    public interface IUsuarioController
    {
        ISaida Autenticar(string email, string senha, JwtTokenConfig tokenConfig);
    }
}
