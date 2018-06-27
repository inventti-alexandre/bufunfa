using JNogueira.Bufunfa.Dominio.Comandos.Entrada;
using JNogueira.Bufunfa.Dominio.Interfaces.Comandos;
using System.Threading.Tasks;

namespace JNogueira.Bufunfa.Dominio.Interfaces.Servicos
{
    /// <summary>
    /// Interface que expõe os serviços relacionádos a entidade "Pessoa"
    /// </summary>
    public interface IPessoaServico
    {
        /// <summary>
        /// Obtém uma pessoa a partir do seu ID
        /// </summary>
        Task<ISaida> ObterPessoaPorId(int idPessoa, int idUsuario);

        /// <summary>
        /// Obtém as pessoas de um usuário.
        /// </summary>
        Task<ISaida> ObterPessoasPorUsuario(int idUsuario);

        /// <summary>
        /// Obtém as pessoas baseadas nos parâmetros de procura
        /// </summary>
        Task<ISaida> ProcurarPessoas(ProcurarPessoaEntrada procurarEntrada);

        /// <summary>
        /// Realiza o cadastro de ums nova pessoa.
        /// </summary>
        Task<ISaida> CadastrarPessoa(CadastrarPessoaEntrada cadastroEntrada);

        /// <summary>
        /// Altera as informações de uma pessoa
        /// </summary>
        Task<ISaida> AlterarPessoa(AlterarPessoaEntrada alterarEntrada);

        /// <summary>
        /// Exclui um período
        /// </summary>
        Task<ISaida> ExcluirPessoa(int idPessoa, int idUsuario);
    }
}
