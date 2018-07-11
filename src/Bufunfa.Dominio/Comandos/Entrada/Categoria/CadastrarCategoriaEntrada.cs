using JNogueira.Bufunfa.Dominio.Interfaces.Comandos;
using JNogueira.Bufunfa.Dominio.Resources;
using JNogueira.Infraestrutura.NotifiqueMe;

namespace JNogueira.Bufunfa.Dominio.Comandos.Entrada
{
    /// <summary>
    /// Comando utilizado para o cadastro de uma nova categoria
    /// </summary>
    public class CadastrarCategoriaEntrada : Notificavel, IEntrada
    {
        /// <summary>
        /// Id do usuário proprietário
        /// </summary>
        public int IdUsuario { get; }

        /// <summary>
        /// Id da categoria pai
        /// </summary>
        public int? IdCategoriaPai { get; }
        
        /// <summary>
        /// Nome da pessoa
        /// </summary>
        public string Nome { get; }

        /// <summary>
        /// Tipo da categoria
        /// </summary>
        public string Tipo { get; }

        public CadastrarCategoriaEntrada(
            int idUsuario,
            string nome,
            string tipo,
            int? idCategoriaPai = null)
        {
            this.IdUsuario      = idUsuario;
            this.Nome           = nome;
            this.Tipo           = tipo?.ToUpper();
            this.IdCategoriaPai = idCategoriaPai;
        }

        public bool Valido()
        {
            this
                .NotificarSeMenorOuIgualA(this.IdUsuario, 0, string.Format(Mensagem.Id_Usuario_Invalido, this.IdUsuario))
                .NotificarSeNuloOuVazio(this.Nome, CategoriaMensagem.Nome_Obrigatorio_Nao_Informado)
                .NotificarSeNuloOuVazio(this.Tipo, CategoriaMensagem.Tipo_Obrigatorio_Nao_Informado);

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
