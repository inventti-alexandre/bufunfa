using JNogueira.Bufunfa.Dominio.Comandos.Entrada;
using JNogueira.Bufunfa.Dominio.Entidades;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JNogueira.Bufunfa.Dominio.Interfaces.Dados
{
    /// <summary>
    /// Define o contrato do repositório de pessoas
    /// </summary>
    public interface IPessoaRepositorio
    {
        /// <summary>
        /// Obtém um período a partir do seu ID
        /// </summary>
        /// <param name="habilitarTracking">Indica que o tracking do EF deverá estar habilitado, permitindo alteração dos dados.</param>
        Task<Pessoa> ObterPorId(int idPessoa, bool habilitarTracking = false);

        /// <summary>
        /// Obtém as pessoas de um usuário.
        /// </summary>
        Task<IEnumerable<Pessoa>> ObterPorUsuario(int idUsuario);

        /// <summary>
        /// Obtém as pessoas baseadas nos parâmetros de procura
        /// </summary>
        Task<IEnumerable<Pessoa>> Procurar(ProcurarPessoaEntrada procurarEntrada);

        /// <summary>
        /// Verifica se um determinado usuário possui uma pessoa com o nome informado
        /// </summary>
        Task<bool> VerificarExistenciaPorNome(int idUsuario, string nome, int? idPessoa = null);

        /// <summary>
        /// Insere uma nova pessoa
        /// </summary>
        Task Inserir(Pessoa pessoa);

        /// <summary>
        /// Atualiza as informações da pessoa
        /// </summary>
        void Atualizar(Pessoa pessoa);

        /// <summary>
        /// Deleta uma pessoa
        /// </summary>
        void Deletar(Pessoa pessoa);
    }
}
