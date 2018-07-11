using JNogueira.Bufunfa.Dominio.Interfaces.Comandos;
using JNogueira.Bufunfa.Dominio.Resources;
using JNogueira.Infraestrutura.NotifiqueMe;

namespace JNogueira.Bufunfa.Dominio.Comandos.Entrada
{
    /// <summary>
    /// Comando utilizado para o alterar uma pessoa
    /// </summary>
    public class AlterarPessoaEntrada : Notificavel, IEntrada
    {
        /// <summary>
        /// Id do usuário proprietário
        /// </summary>
        public int IdUsuario { get; }

        /// <summary>
        /// ID da pessoa
        /// </summary>
        public int IdPessoa { get; }

        /// <summary>
        /// Nome da pessoa
        /// </summary>
        public string Nome { get; }

        public AlterarPessoaEntrada(
            int idPessoa,
            string nome,
            int idUsuario)
        {
            this.IdPessoa  = idPessoa;
            this.Nome      = nome;
            this.IdUsuario = idUsuario;
        }

        public bool Valido()
        {
            this
                .NotificarSeMenorOuIgualA(this.IdPessoa, 0, string.Format(PessoaMensagem.Id_Pessoa_Invalido, this.IdPessoa))
                .NotificarSeMenorOuIgualA(this.IdUsuario, 0, string.Format(Mensagem.Id_Usuario_Invalido, this.IdUsuario))
                .NotificarSeNuloOuVazio(this.Nome, PessoaMensagem.Nome_Obrigatorio_Nao_Informado);

            if (!string.IsNullOrEmpty(this.Nome))
                this.NotificarSePossuirTamanhoSuperiorA(this.Nome, 200, PessoaMensagem.Nome_Tamanho_Maximo_Excedido);

            return !this.Invalido;
        }
    }
}
