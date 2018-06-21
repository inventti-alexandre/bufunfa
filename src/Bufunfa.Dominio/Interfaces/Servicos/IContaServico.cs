using JNogueira.Bufunfa.Dominio.Comandos.Entrada;
using JNogueira.Bufunfa.Dominio.Interfaces.Comandos;
using System.Threading.Tasks;

namespace JNogueira.Bufunfa.Dominio.Interfaces.Servicos
{
    /// <summary>
    /// Interface que expõe os serviços relacionádos a entidade "Conta"
    /// </summary>
    public interface IContaServico
    {
        /// <summary>
        /// Obtém uma conta a partir do seu ID
        /// </summary>
        Task<ISaida> ObterContaPorId(int idConta, int idUsuario);

        /// <summary>
        /// Obtém as contas de um usuário.
        /// </summary>
        Task<ISaida> ObterContasPorUsuario(int idUsuario);

        /// <summary>
        /// Realiza o cadastro de uma nova conta.
        /// </summary>
        Task<ISaida> CadastrarConta(CadastrarContaEntrada cadastroEntrada);

        /// <summary>
        /// Altera as informações da conta
        /// </summary>
        Task<ISaida> AlterarConta(AlterarContaEntrada alterarEntrada);

        /// <summary>
        /// Exclui uma conta
        /// </summary>
        Task<ISaida> ExcluirConta(int idConta, int idUsuario);
    }
}
