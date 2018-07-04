using JNogueira.Bufunfa.Dominio.Entidades;
using JNogueira.Bufunfa.Dominio.Resources;
using JNogueira.Infraestrutura.NotifiqueMe;

namespace JNogueira.Bufunfa.Dominio.Comandos.Entrada
{
    /// <summary>
    /// Classe com opções de filtro para procura de categorias
    /// </summary>
    public class ProcurarCategoriaEntrada : ProcurarEntrada
    {
        public string Nome { get; set; }

        public int? IdCategoriaPai { get; set; }

        public string Tipo { get; set; }

        public ProcurarCategoriaEntrada(int idUsuario, string ordenarPor, string ordenarSentido, int? paginaIndex = null, int? paginaTamanho = null)
            : base(idUsuario, string.IsNullOrEmpty(ordenarPor) ? "Nome" : ordenarPor, string.IsNullOrEmpty(ordenarSentido) ? "ASC" : ordenarSentido, paginaIndex, paginaTamanho)
        {
            
        }

        public override bool Valido()
        {
            base.Valido();

            this.NotificarSeNulo(typeof(Categoria).GetProperty(this.OrdenarPor), string.Format(Mensagem.Paginacao_OrdernarPor_Propriedade_Nao_Existe, this.OrdenarPor));

            if (!string.IsNullOrEmpty(this.Nome))
                this.NotificarSePossuirTamanhoSuperiorA(this.Nome, 100, CategoriaMensagem.Nome_Tamanho_Maximo_Excedido);

            if (!string.IsNullOrEmpty(this.Tipo))
                this.NotificarSeVerdadeiro(this.Tipo != "D" && this.Tipo != "C", string.Format(CategoriaMensagem.Tipo_Invalido, this.Tipo));

            if (this.IdCategoriaPai.HasValue)
                this.NotificarSeMenorQue(this.IdCategoriaPai.Value, 1, string.Format(CategoriaMensagem.Id_Categoria_Pai_Invalido, this.IdCategoriaPai.Value));

            return !this.Invalido;
        }
    }
}
