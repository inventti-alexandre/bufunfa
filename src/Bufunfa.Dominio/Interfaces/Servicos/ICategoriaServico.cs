using JNogueira.Bufunfa.Dominio.Comandos.Entrada;
using JNogueira.Bufunfa.Dominio.Interfaces.Comandos;
using System.Threading.Tasks;

namespace JNogueira.Bufunfa.Dominio.Interfaces.Servicos
{
    /// <summary>
    /// Interface que expõe os serviços relacionádos a entidade "Categoria"
    /// </summary>
    public interface ICategoriaServico
    {
        /// <summary>
        /// Obtém uma categoria a partir do seu ID
        /// </summary>
        Task<ISaida> ObterCategoriaPorId(int idCategoria, int idUsuario);

        /// <summary>
        /// Obtém as categorias de um usuário.
        /// </summary>
        Task<ISaida> ObterCategoriasPorUsuario(int idUsuario);

        /// <summary>
        /// Obtém as categorias baseados nos parâmetros de procura
        /// </summary>
        Task<ISaida> ProcurarCategorias(ProcurarCategoriaEntrada procurarEntrada);

        /// <summary>
        /// Realiza o cadastro de um novo período.
        /// </summary>
        Task<ISaida> CadastrarCategoria(CadastrarCategoriaEntrada cadastroEntrada);

        /// <summary>
        /// Altera as informações do período
        /// </summary>
        Task<ISaida> AlterarCategoria(AlterarCategoriaEntrada alterarEntrada);

        /// <summary>
        /// Exclui um período
        /// </summary>
        Task<ISaida> ExcluirCategoria(int idCategoria, int idUsuario);
    }
}
