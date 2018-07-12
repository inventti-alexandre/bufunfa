using JNogueira.Bufunfa.Dominio.Comandos.Entrada;
using System.Collections.Generic;
using System.Linq;

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
        public int Id { get; private set; }

        /// <summary>
        /// Id do usuário proprietário
        /// </summary>
        public int IdUsuario { get; private set; }

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

        /// <summary>
        /// Indica se a categoria é pai de pelo menos uma outra categoria.
        /// </summary>
        public bool Pai
        {
            get
            {
                return this.CategoriasFilha != null && this.CategoriasFilha.Any();
            }
        }

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
        /// Obtém o caminho da categoria
        /// </summary>
        public string ObterCaminho()
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

        public void Alterar(AlterarCategoriaEntrada alterarEntrada)
        {
            if (!alterarEntrada.Valido() || alterarEntrada.IdCategoria != this.Id)
                return;

            this.Nome           = alterarEntrada.Nome;
            this.IdCategoriaPai = alterarEntrada.IdCategoriaPai;
            this.Tipo           = alterarEntrada.Tipo;
        }

        public override string ToString()
        {
            return $"{this.Nome} ({this.ObterCaminho()})";
        }

        private static string ObterNomeCategoriaPai(Categoria categoria)
        {
            if (categoria.IdCategoriaPai.HasValue && categoria.CategoriaPai != null)
                return $"{ObterNomeCategoriaPai(categoria.CategoriaPai)} » {categoria.Nome}";

            return $"{categoria.ObterDescricaoTipo().ToUpper()} » " + categoria.Nome;
        }
    }
}
