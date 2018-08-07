using JNogueira.Bufunfa.Dominio.Comandos.Entrada;
using JNogueira.Bufunfa.Dominio.Entidades;
using JNogueira.Infraestrutura.NotifiqueMe;
using System;
using System.Threading.Tasks;

namespace JNogueira.Bufunfa.Dominio.Interfaces.Dados
{
    /// <summary>
    /// Define o contrato do repositório de anexos
    /// </summary>
    public interface IAnexoRepositorio : INotificavel
    {
        /// <summary>
        /// Obtém um anexo a partir do seu ID
        /// </summary>
        /// <param name="habilitarTracking">Indica que o tracking do EF deverá estar habilitado, permitindo alteração dos dados.</param>
        Task<Anexo> ObterPorId(int idAnexo, bool habilitarTracking = false);

        /// <summary>
        /// Insere um novo anexo no banco de dados e realiza o upload do arquivo para o Google Drive.
        /// </summary>
        Task<Anexo> Inserir(DateTime dataLancamento, CadastrarAnexoEntrada cadastroEntrada);

        /// <summary>
        /// Deleta um anexo
        /// </summary>
        Task Deletar(Anexo anexo);
    }
}
