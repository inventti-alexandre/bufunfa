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

            this.Validar();
        }

        private void Validar()
        {
            this
                .NotificarSeMenorOuIgualA(this.IdPessoa, 0, PessoaMensagem.Id_Pessoa_Invalido)
                .NotificarSeMenorOuIgualA(this.IdUsuario, 0, Mensagem.Id_Usuario_Invalido)
                .NotificarSeNuloOuVazio(this.Nome, PessoaMensagem.Nome_Obrigatorio_Nao_Informado);

            if (!string.IsNullOrEmpty(this.Nome))
                this.NotificarSePossuirTamanhoSuperiorA(this.Nome, 200, PessoaMensagem.Nome_Tamanho_Maximo_Excedido);
        }
    }
}
