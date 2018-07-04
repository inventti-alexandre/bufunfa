using JNogueira.Bufunfa.Dominio.Interfaces.Comandos;
using JNogueira.Bufunfa.Dominio.Resources;
using JNogueira.Infraestrutura.NotifiqueMe;

namespace JNogueira.Bufunfa.Dominio.Comandos.Entrada
{
    /// <summary>
    /// Comando utilizado para o alterar uma categoria
    /// </summary>
    public class AlterarCategoriaEntrada : Notificavel, IEntrada
    {
        /// <summary>
        /// Id do usuário proprietário
        /// </summary>
        public int IdUsuario { get; }

        /// <summary>
        /// ID da categoria
        /// </summary>
        public int IdCategoria { get; }

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

        public AlterarCategoriaEntrada(int idCategoria, string nome, int? idCategoriaPai, string tipo, int idUsuario)
        {
            this.IdCategoria = idCategoria;
            this.IdCategoriaPai = idCategoriaPai;
            this.Nome = nome;
            this.Tipo = tipo?.ToUpper();
            
            this.IdUsuario = idUsuario;
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
            {
                this.NotificarSeMenorQue(this.IdCategoriaPai.Value, 1, string.Format(CategoriaMensagem.Id_Categoria_Pai_Invalido, this.IdCategoriaPai.Value));
                this.NotificarSeIguais(this.IdCategoriaPai.Value, this.IdCategoria, CategoriaMensagem.Id_Categoria_Pai_Id_Categoria_Nao_Podem_Ser_Iguais);
            }

            return !this.Invalido;
        }
    }
}
