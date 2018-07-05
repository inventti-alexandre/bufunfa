using JNogueira.Bufunfa.Dominio.Comandos.Entrada;
using JNogueira.Bufunfa.Dominio.Interfaces.Comandos;
using System.Threading.Tasks;

namespace JNogueira.Bufunfa.Dominio.Interfaces.Servicos
{
    /// <summary>
    /// Interface que expõe os serviços relacionádos a entidade "CartaoCredito"
    /// </summary>
    public interface ICartaoCreditoServico
    {
        /// <summary>
        /// Obtém um cartão a partir do seu ID
        /// </summary>
        Task<ISaida> ObterCartaoCreditoPorId(int idCartao, int idUsuario);

        /// <summary>
        /// Obtém os cartões de um usuário.
        /// </summary>
        Task<ISaida> ObterCartoesCreditoPorUsuario(int idUsuario);

        /// <summary>
        /// Realiza o cadastro de um novo cartão.
        /// </summary>
        Task<ISaida> CadastrarCartaoCredito(CadastrarCartaoCreditoEntrada cadastroEntrada);

        /// <summary>
        /// Altera as informações do cartão.
        /// </summary>
        Task<ISaida> AlterarCartaoCredito(AlterarCartaoCreditoEntrada alterarEntrada);

        /// <summary>
        /// Exclui um cartão
        /// </summary>
        Task<ISaida> ExcluirCartaoCredito(int idCartao, int idUsuario);
    }
}
