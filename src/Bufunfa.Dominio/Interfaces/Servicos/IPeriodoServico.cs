using JNogueira.Bufunfa.Dominio.Comandos.Entrada;
using JNogueira.Bufunfa.Dominio.Interfaces.Comandos;

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
        ISaida ObterPeriodoPorId(int idPeriodo, int idUsuario);

        /// <summary>
        /// Obtém os períodos de um usuário.
        /// </summary>
        ISaida ObterPeriodosPorUsuario(int idUsuario);

        /// <summary>
        /// Realiza o cadastro de um novo período.
        /// </summary>
        ISaida CadastrarPeriodo(CadastrarPeriodoEntrada cadastroEntrada);

        /// <summary>
        /// Altera as informações do período
        /// </summary>
        ISaida AlterarPeriodo(AlterarPeriodoEntrada alterarEntrada);

        /// <summary>
        /// Exclui um período
        /// </summary>
        ISaida ExcluirPeriodo(int idPeriodo, int idUsuario);
    }
}
