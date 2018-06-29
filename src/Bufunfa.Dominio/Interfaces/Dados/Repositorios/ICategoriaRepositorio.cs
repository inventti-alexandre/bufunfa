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
        /// Obtém um período a partir do seu ID
        /// </summary>
        /// <param name="habilitarTracking">Indica que o tracking do EF deverá estar habilitado, permitindo alteração dos dados.</param>
        Task<Categoria> ObterPorId(int idCategoria, bool habilitarTracking = false);

        /// <summary>
        /// Obtém as pessoas de um usuário.
        /// </summary>
        Task<IEnumerable<Categoria>> ObterPorUsuario(int idUsuario);

        /// <summary>
        /// Obtém as pessoas baseadas nos parâmetros de procura
        /// </summary>
        //Task<ProcurarSaida> Procurar(ProcurarCategoriaEntrada procurarEntrada);

        /// <summary>
        /// Verifica se um determinado usuário possui uma pessoa com o nome informado
        /// </summary>
        //Task<bool> VerificarExistenciaPorNome(int idUsuario, string nome, int? idCategoria = null);

        /// <summary>
        /// Insere uma nova pessoa
        /// </summary>
        Task Inserir(Categoria pessoa);

        /// <summary>
        /// Atualiza as informações da pessoa
        /// </summary>
        void Atualizar(Categoria pessoa);

        /// <summary>
        /// Deleta uma pessoa
        /// </summary>
        void Deletar(Categoria pessoa);
    }
}
