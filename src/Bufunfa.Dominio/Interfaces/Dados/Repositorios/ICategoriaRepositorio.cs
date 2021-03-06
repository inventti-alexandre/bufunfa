﻿using JNogueira.Bufunfa.Dominio.Comandos.Entrada;
using JNogueira.Bufunfa.Dominio.Comandos.Saida;
using JNogueira.Bufunfa.Dominio.Entidades;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JNogueira.Bufunfa.Dominio.Interfaces.Dados
{
    /// <summary>
    /// Define o contrato do repositório de categorias
    /// </summary>
    public interface ICategoriaRepositorio
    {
        /// <summary>
        /// Obtém uma categoria a partir do seu ID
        /// </summary>
        /// <param name="habilitarTracking">Indica que o tracking do EF deverá estar habilitado, permitindo alteração dos dados.</param>
        Task<Categoria> ObterPorId(int idCategoria, bool habilitarTracking = false);

        /// <summary>
        /// Obtém as categorias de um usuário.
        /// </summary>
        Task<IEnumerable<Categoria>> ObterPorUsuario(int idUsuario);

        /// <summary>
        /// Obtém as categorias baseadas nos parâmetros de procura
        /// </summary>
        Task<ProcurarSaida> Procurar(ProcurarCategoriaEntrada procurarEntrada);

        /// <summary>
        /// Verifica se um determinado usuário possui uma categoria com o nome e tipo informado
        /// </summary>
        Task<bool> VerificarExistenciaPorNomeTipo(int idUsuario, string nome, string tipo, int? idCategoria = null);

        /// <summary>
        /// Verifica a existência de uma categoria a partir do seu ID.
        /// </summary>
        Task<bool> VerificarExistenciaPorId(int idUsuario, int idCategoria);

        /// <summary>
        /// Insere uma nova categoria
        /// </summary>
        Task Inserir(Categoria categoria);

        /// <summary>
        /// Atualiza as informações da categoria
        /// </summary>
        void Atualizar(Categoria categoria);

        /// <summary>
        /// Deleta uma categoria
        /// </summary>
        void Deletar(Categoria categoria);
    }
}
