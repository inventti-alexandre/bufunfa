using JNogueira.Bufunfa.Dominio.Comandos.Entrada;
using JNogueira.Bufunfa.Dominio.Interfaces.Comandos;
using System.Threading.Tasks;

namespace JNogueira.Bufunfa.Dominio.Interfaces.Servicos
{
    /// <summary>
    /// Interface que expõe os serviços relacionádos a entidade "Lançamento"
    /// </summary>
    public interface ILancamentoServico
    {
        /// <summary>
        /// Obtém um lançamento a partir do seu ID
        /// </summary>
        Task<ISaida> ObterLancamentoPorId(int idLancamento, int idUsuario);

        /// <summary>
        /// Obtém os lançamentos baseadas nos parâmetros de procura
        /// </summary>
        Task<ISaida> ProcurarLancamentos(ProcurarLancamentoEntrada procurarEntrada);

        /// <summary>
        /// Realiza o cadastro de um novo lançamento.
        /// </summary>
        Task<ISaida> CadastrarLancamento(CadastrarLancamentoEntrada cadastroEntrada);

        /// <summary>
        /// Altera as informações de um lançamento.
        /// </summary>
        Task<ISaida> AlterarLancamento(AlterarLancamentoEntrada alterarEntrada);

        /// <summary>
        /// Exclui um lançamento.
        /// </summary>
        Task<ISaida> ExcluirLancamento(int idLancamento, int idUsuario);

        /// <summary>
        /// Realiza o cadastro de um novo anexo para um lançamento.
        /// </summary>
        Task<ISaida> CadastrarAnexo(CadastrarAnexoEntrada cadastroEntrada);

        /// <summary>
        /// Exclui um anexo de um lançamento.
        /// </summary>
        Task<ISaida> ExcluirAnexo(int idLancamento, int idUsuario);
    }
}
