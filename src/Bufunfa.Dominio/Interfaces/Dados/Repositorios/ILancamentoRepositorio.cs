using JNogueira.Bufunfa.Dominio.Comandos.Entrada;
using JNogueira.Bufunfa.Dominio.Comandos.Saida;
using JNogueira.Bufunfa.Dominio.Entidades;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JNogueira.Bufunfa.Dominio.Interfaces.Dados
{
    /// <summary>
    /// Define o contrato do repositório de lançamentos
    /// </summary>
    public interface ILancamentoRepositorio
    {
        /// <summary>
        /// Obtém um lançamento a partir do seu ID
        /// </summary>
        /// <param name="habilitarTracking">Indica que o tracking do EF deverá estar habilitado, permitindo alteração dos dados.</param>
        Task<Lancamento> ObterPorId(int idLancamento, bool habilitarTracking = false);

        /// <summary>
        /// Obtém os lançamentos de um usuário.
        /// </summary>
        Task<IEnumerable<Lancamento>> ObterPorUsuario(int idUsuario);

        /// <summary>
        /// Obtém os lançamentos baseados nos parâmetros de procura
        /// </summary>
        Task<ProcurarSaida> Procurar(ProcurarLancamentoEntrada procurarEntrada);

        /// <summary>
        /// Insere uma novo lançamento
        /// </summary>
        Task Inserir(Lancamento lancamento);

        /// <summary>
        /// Atualiza as informações do lançamento
        /// </summary>
        void Atualizar(Lancamento lancamento);

        /// <summary>
        /// Deleta um lançamento
        /// </summary>
        void Deletar(Lancamento lancamento);
    }
}
