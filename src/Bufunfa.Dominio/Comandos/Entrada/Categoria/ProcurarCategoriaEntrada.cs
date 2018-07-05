using JNogueira.Bufunfa.Dominio.Resources;
using JNogueira.Infraestrutura.NotifiqueMe;

namespace JNogueira.Bufunfa.Dominio.Comandos.Entrada
{
    /// <summary>
    /// Classe com opções de filtro para procura de categorias
    /// </summary>
    public class ProcurarCategoriaEntrada : Notificavel
    {
        public int IdUsuario { get; private set; }

        public string Nome { get; set; }

        public int? IdCategoriaPai { get; set; }

        public string Tipo { get; set; }

        public string Caminho { get; set; }

        public ProcurarCategoriaEntrada(int idUsuario)
        {
            this.IdUsuario = idUsuario;
        }

        public bool Valido()
        {
            this.NotificarSeMenorOuIgualA(this.IdUsuario, 0, string.Format(Mensagem.Id_Usuario_Invalido, this.IdUsuario));

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
