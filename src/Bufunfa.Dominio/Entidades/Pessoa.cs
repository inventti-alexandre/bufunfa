using JNogueira.Bufunfa.Dominio.Comandos.Entrada;

namespace JNogueira.Bufunfa.Dominio.Entidades
{
    /// <summary>
    /// Classe que representa uma pessoa
    /// </summary>
    public class Pessoa
    {
        /// <summary>
        /// ID da pessoa
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Id do usuário proprietário
        /// </summary>
        public int IdUsuario { get; private set; }

        /// <summary>
        /// Nome da período
        /// </summary>
        public string Nome { get; private set; }

        private Pessoa()
        {
        }

        public Pessoa(CadastrarPessoaEntrada cadastrarEntrada)
        {
            if (!cadastrarEntrada.Valido())
                return;

            this.IdUsuario = cadastrarEntrada.IdUsuario;
            this.Nome = cadastrarEntrada.Nome;
        }

        public void Alterar(AlterarPessoaEntrada alterarEntrada)
        {
            if (!alterarEntrada.Valido() || alterarEntrada.IdPessoa != this.Id)
                return;

            this.Nome = alterarEntrada.Nome;
        }

        public override string ToString()
        {
            return this.Nome;
        }
    }
}