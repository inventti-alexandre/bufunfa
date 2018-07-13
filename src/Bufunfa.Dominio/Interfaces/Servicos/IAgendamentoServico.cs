using JNogueira.Bufunfa.Dominio.Comandos.Entrada;
using JNogueira.Bufunfa.Dominio.Interfaces.Comandos;
using System.Threading.Tasks;

namespace JNogueira.Bufunfa.Dominio.Interfaces.Servicos
{
    /// <summary>
    /// Interface que expõe os serviços relacionádos a entidade "Agendamento"
    /// </summary>
    public interface IAgendamentoServico
    {
        /// <summary>
        /// Obtém um agendamento a partir do seu ID
        /// </summary>
        Task<ISaida> ObterAgendamentoPorId(int idAgendamento, int idUsuario);

        /// <summary>
        /// Obtém os agendamentos baseadas nos parâmetros de procura
        /// </summary>
        Task<ISaida> ProcurarAgendamentos(ProcurarAgendamentoEntrada procurarEntrada);

        /// <summary>
        /// Realiza o cadastro de um novo agendamento.
        /// </summary>
        Task<ISaida> CadastrarAgendamento(CadastrarAgendamentoEntrada cadastroEntrada);

        /// <summary>
        /// Altera as informações de um agendamento.
        /// </summary>
        Task<ISaida> AlterarAgendamento(AlterarAgendamentoEntrada alterarEntrada);

        /// <summary>
        /// Exclui um agendamento.
        /// </summary>
        Task<ISaida> ExcluirAgendamento(int idAgendamento, int idUsuario);

        /// <summary>
        /// Realiza o cadastro de uma nova parcela.
        /// </summary>
        Task<ISaida> CadastrarParcela(CadastrarParcelaEntrada cadastroEntrada);

        /// <summary>
        /// Altera as informações de uma parcela.
        /// </summary>
        Task<ISaida> AlterarParcela(AlterarParcelaEntrada alterarEntrada);

        /// <summary>
        /// Lança uma parcela.
        /// </summary>
        Task<ISaida> LancarParcela(LancarParcelaEntrada lancarEntrada);

        /// <summary>
        /// Descarta uma parcela.
        /// </summary>
        Task<ISaida> DescartarParcela(DescartarParcelaEntrada descartarEntrada);

        /// <summary>
        /// Exclui uma parcela.
        /// </summary>
        Task<ISaida> ExcluirParcela(int idParcela, int idUsuario);
    }
}
