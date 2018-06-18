using JNogueira.Bufunfa.Dominio.Entidades;
using System;
using System.Collections.Generic;

namespace JNogueira.Bufunfa.Dominio.Interfaces.Dados
{
    /// <summary>
    /// Define o contrato do repositório de períodos
    /// </summary>
    public interface IPeriodoRepositorio
    {
        /// <summary>
        /// Obtém um período a partir do seu ID
        /// </summary>
        /// <param name="habilitarTracking">Indica que o tracking do EF deverá estar habilitado, permitindo alteração dos dados.</param>
        Periodo ObterPorId(int idPeriodo, bool habilitarTracking = false);
        
        /// <summary>
        /// Insere um novo período
        /// </summary>
        void Inserir(Periodo periodo);

        /// <summary>
        /// Atualiza as informações do período
        /// </summary>
        void Atualizar(Periodo periodo);

        /// <summary>
        /// Deleta um períoodo
        /// </summary>
        void Deletar(int idPeriodo);

        /// <summary>
        /// Verifica se um determinado usuário possui um período com as datas de início e fim informados
        /// </summary>
        bool VerificarExistenciaPorDataInicioFim(int idUsuario, DateTime dataInicio, DateTime dataFim, int? idPeriodo = null);

        /// <summary>
        /// Obtém os períodos de um usuário.
        /// </summary>
        IEnumerable<Periodo> ObterPorUsuario(int idUsuario);
    }
}
