﻿using JNogueira.Bufunfa.Dominio.Comandos.Entrada;
using System.Collections.Generic;

namespace JNogueira.Bufunfa.Dominio.Entidades
{
    /// <summary>
    /// Classe que representa uma categoria
    /// </summary>
    public class Categoria
    {
        /// <summary>
        /// ID da categoria
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Id do usuário proprietário
        /// </summary>
        public int IdUsuario { get; }

        /// <summary>
        /// ID da categoria pai
        /// </summary>
        public int? IdCategoriaPai { get; private set; }

        /// <summary>
        /// Nome da categoria
        /// </summary>
        public string Nome { get; private set; }

        /// <summary>
        /// Tipo da categoria
        /// </summary>
        public string Tipo { get; private set; }

        /// <summary>
        /// Categoria pai
        /// </summary>
        public Categoria CategoriaPai { get; private set; }

        /// <summary>
        /// Categorias filha
        /// </summary>
        public IEnumerable<Categoria> CategoriasFilha { get; private set; }

        private Categoria()
        {
            this.CategoriasFilha = new List<Categoria>();
        }

        /// <summary>
        /// Obtém a descrição do tipo da categoria
        /// </summary>
        public string ObterDescricaoTipo()
        {
            switch (this.Tipo)
            {
                case "C": return "Crédito";
                case "D": return "Débito";
                default: return string.Empty;
            }
        }

        /// <summary>
        /// Obtém a árvore da categoria
        /// </summary>
        public string ObterArvore()
        {
            return this.CategoriaPai != null
                    ? ObterNomeCategoriaPai(this)
                    : $"{this.ObterDescricaoTipo().ToUpper()} » " + this.Nome;
        }

        public Categoria(CadastrarCategoriaEntrada cadastrarEntrada)
        {
            if (!cadastrarEntrada.Valido())
                return;

            this.IdUsuario      = cadastrarEntrada.IdUsuario;
            this.IdCategoriaPai = cadastrarEntrada.IdCategoriaPai;
            this.Nome           = cadastrarEntrada.Nome;
            this.Tipo           = cadastrarEntrada.Tipo;
        }

        public override string ToString()
        {
            return $"{this.Nome} ({this.ObterArvore()})";
        }

        private static string ObterNomeCategoriaPai(Categoria categoria)
        {
            if (categoria.IdCategoriaPai.HasValue && categoria.CategoriaPai != null)
                return $"{ObterNomeCategoriaPai(categoria.CategoriaPai)} » {categoria.Nome}";

            return $"{categoria.ObterDescricaoTipo().ToUpper()} » " + categoria.Nome;
        }
    }
}
