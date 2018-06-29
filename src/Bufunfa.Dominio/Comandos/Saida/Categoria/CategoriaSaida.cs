using JNogueira.Bufunfa.Dominio.Entidades;
using System.Collections.Generic;
using System.Linq;

namespace JNogueira.Bufunfa.Dominio.Comandos.Saida
{
    /// <summary>
    /// Comando de sáida para as informações de uma categoria
    /// </summary>
    public class CategoriaSaida
    {
        /// <summary>
        /// ID da categoria
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Nome da categoria
        /// </summary>
        public string Nome { get;  }

        /// <summary>
        /// Tipo da categoria
        /// </summary>
        public string Tipo { get; }

        /// <summary>
        /// Árvore da categoria
        /// </summary>
        public string Arvore { get; }

        /// <summary>
        /// Categoria pai
        /// </summary>
        public object CategoriaPai { get; }

        /// <summary>
        /// Categorias filha
        /// </summary>
        public IEnumerable<object> CategoriasFilha { get; }
       
        public CategoriaSaida(Categoria categoria)
        {
            if (categoria == null)
                return;

            this.Id     = categoria.Id;
            this.Nome   = categoria.Nome;
            this.Tipo   = categoria.Tipo;
            this.Arvore = categoria.ObterArvore();

            if (categoria.CategoriaPai != null)
            {
                this.CategoriaPai = new
                {
                    Id             = categoria.CategoriaPai.Id,
                    IdCategoriaPai = categoria.CategoriaPai.IdCategoriaPai,
                    Nome           = categoria.CategoriaPai.Nome,
                    Tipo           = categoria.CategoriaPai.Tipo,
                    Arvore         = categoria.CategoriaPai.ObterArvore()
                };
            }

            this.CategoriasFilha = categoria.CategoriasFilha.Select(x => new
            {
                Id     = x.Id,
                Nome   = x.Nome,
                Tipo   = x.Tipo,
                Arvore = x.ObterArvore()
            }).ToList();
        }
        
        public override string ToString()
        {
            return $"{this.Nome} ({this.Arvore})";
        }
    }
}
