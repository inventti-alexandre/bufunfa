using JNogueira.Bufunfa.Dominio.Comandos.Entrada;
using JNogueira.Bufunfa.Dominio.Interfaces.Comandos;
using System.Threading.Tasks;

namespace JNogueira.Bufunfa.Dominio.Interfaces.Servicos
{
    /// <summary>
    /// Interface que expõe os serviços relacionádos a entidade "Período"
    /// </summary>
    public interface IPeriodoServico
    {
        /// <summary>
        /// Obtém um período a partir do seu ID
        /// </summary>
        Task<ISaida> ObterPeriodoPorId(int idPeriodo, int idUsuario);

        /// <summary>
        /// Obtém os períodos de um usuário.
        /// </summary>
        Task<ISaida> ObterPeriodosPorUsuario(int idUsuario);

        /// <summary>
        /// Obtém os períodos baseados nos parâmetros de procura
        /// </summary>
        Task<ISaida> ProcurarPeriodos(ProcurarPeriodoEntrada procurarEntrada);

        /// <summary>
        /// Realiza o cadastro de um novo período.
        /// </summary>
        Task<ISaida> CadastrarPeriodo(CadastrarPeriodoEntrada cadastroEntrada);

        /// <summary>
        /// Altera as informações do período
        /// </summary>
        Task<ISaida> AlterarPeriodo(AlterarPeriodoEntrada alterarEntrada);

        /// <summary>
        /// Exclui um período
        /// </summary>
        Task<ISaida> ExcluirPeriodo(int idPeriodo, int idUsuario);
    }
}
