using JNogueira.Bufunfa.Dominio.Entidades;
using System.Collections.Generic;

namespace JNogueira.Bufunfa.Dominio.Interfaces.Dados
{
    /// <summary>
    /// Define o contrato do repositório de contas
    /// </summary>
    public interface IContaRepositorio
    {
        /// <summary>
        /// Obtém uma conta a partir do seu ID
        /// </summary>
        /// <param name="habilitarTracking">Indica que o tracking do EF deverá estar habilitado, permitindo alteração dos dados.</param>
        Conta ObterPorId(int idConta, bool habilitarTracking = false);
        
        /// <summary>
        /// Insere uma nova conta
        /// </summary>
        void Inserir(Conta conta);

        /// <summary>
        /// Atualiza as informações da conta
        /// </summary>
        void Atualizar(Conta conta);

        /// <summary>
        /// Deleta uma conta
        /// </summary>
        void Deletar(int idConta);

        /// <summary>
        /// Verifica se um determinado usuário possui uma conta com o nome informado
        /// </summary>
        bool VerificarExistenciaPorNome(int idUsuario, string nome, int? idConta = null);

        /// <summary>
        /// Obtém as contas de um usuário.
        /// </summary>
        IEnumerable<Conta> ObterPorUsuario(int idUsuario);
    }
}
