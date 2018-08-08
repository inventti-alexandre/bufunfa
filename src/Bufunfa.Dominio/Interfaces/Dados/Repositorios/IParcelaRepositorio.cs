using JNogueira.Bufunfa.Dominio.Entidades;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JNogueira.Bufunfa.Dominio.Interfaces.Dados
{
    /// <summary>
    /// Define o contrato do repositório de parcelas
    /// </summary>
    public interface IParcelaRepositorio
    {
        /// <summary>
        /// Obtém uma parcela a partir do seu ID
        /// </summary>
        /// <param name="habilitarTracking">Indica que o tracking do EF deverá estar habilitado, permitindo alteração dos dados.</param>
        Task<Parcela> ObterPorId(int idParcela, bool habilitarTracking = false);

        /// <summary>
        /// Obtém as parcelas de um usuário.
        /// </summary>
        Task<IEnumerable<Parcela>> ObterPorUsuario(int idUsuario);

        /// <summary>
        /// Insere uma nova parcela
        /// </summary>
        Task Inserir(Parcela parcela);

        /// <summary>
        /// Insere várias parcelas
        /// </summary>
        Task Inserir(IEnumerable<Parcela> parcelas);

        /// <summary>
        /// Atualiza as informações da parcel
        /// </summary>
        void Atualizar(Parcela parcela);

        /// <summary>
        /// Deleta uma parcela
        /// </summary>
        void Deletar(Parcela parcela);

        /// <summary>
        /// Deleta várias parcelas
        /// </summary>
        void Deletar(IEnumerable<Parcela> parcelas);
    }
}
