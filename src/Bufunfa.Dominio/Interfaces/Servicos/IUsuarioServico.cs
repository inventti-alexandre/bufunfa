using JNogueira.Bufunfa.Dominio.Comandos.Entrada;
using JNogueira.Bufunfa.Dominio.Interfaces.Comandos;

namespace JNogueira.Bufunfa.Dominio.Interfaces.Servicos
{
    /// <summary>
    /// Interface que expõe os serviços relacionádos a entidade "Usuário"
    /// </summary>
    public interface IUsuarioServico
    {
        /// <summary>
        /// Realiza a autenticação de um usuário
        /// </summary>
        ISaida Autenticar(AutenticarUsuarioEntrada autenticacaoEntrada);

        /// <summary>
        /// Obtém um usuário a partir do seu e-mail
        /// </summary>
        ISaida ObterUsuarioPorEmail(string email);
    }
}
