using JNogueira.Bufunfa.Dominio.Comandos.Entrada;
using JNogueira.Bufunfa.Dominio.Comandos.Saida;
using JNogueira.Bufunfa.Dominio.Entidades;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JNogueira.Bufunfa.Dominio.Interfaces.Dados
{
    /// <summary>
    /// Define o contrato do repositório de agendamentos
    /// </summary>
    public interface IAgendamentoRepositorio
    {
        /// <summary>
        /// Obtém um agendamento a partir do seu ID
        /// </summary>
        /// <param name="habilitarTracking">Indica que o tracking do EF deverá estar habilitado, permitindo alteração dos dados.</param>
        Task<Agendamento> ObterPorId(int idAgendamento, bool habilitarTracking = false);

        /// <summary>
        /// Obtém os agendamentos de um usuário.
        /// </summary>
        Task<IEnumerable<Agendamento>> ObterPorUsuario(int idUsuario);

        /// <summary>
        /// Obtém os agendamentos baseados nos parâmetros de procura
        /// </summary>
        Task<ProcurarSaida> Procurar(ProcurarAgendamentoEntrada procurarEntrada);

        /// <summary>
        /// Insere um novo agendamento
        /// </summary>
        Task Inserir(Agendamento agendamento);

        /// <summary>
        /// Atualiza as informações do agendamento
        /// </summary>
        void Atualizar(Agendamento agendamento);

        /// <summary>
        /// Deleta um agendamento
        /// </summary>
        void Deletar(Agendamento agendamento);
    }
}
