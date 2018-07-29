using JNogueira.Bufunfa.Dominio.Entidades;
using System.Threading.Tasks;

namespace JNogueira.Bufunfa.Dominio.Interfaces.Dados
{
    /// <summary>
    /// Define o contrato do repositório de anexos
    /// </summary>
    public interface IAnexoRepositorio
    {
        /// <summary>
        /// Obtém um anexo a partir do seu ID
        /// </summary>
        /// <param name="habilitarTracking">Indica que o tracking do EF deverá estar habilitado, permitindo alteração dos dados.</param>
        Task<Anexo> ObterPorId(int idAnexo, bool habilitarTracking = false);

        /// <summary>
        /// Insere um novo anexo
        /// </summary>
        Task Inserir(Anexo anexo);

        /// <summary>
        /// Deleta um anexo
        /// </summary>
        void Deletar(Anexo anexo);
    }
}
