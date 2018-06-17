using JNogueira.Bufunfa.Dominio.Interfaces.Comandos;
using JNogueira.Infraestrutura.NotifiqueMe;

namespace JNogueira.Bufunfa.Dominio.Comandos.Entrada
{
    /// <summary>
    /// Comando utilizado para o cadastro de uma nova conta
    /// </summary>
    public class CadastrarContaEntrada : Notificavel, IEntrada
    {
        /// <summary>
        /// Id do usuário proprietário da conta
        /// </summary>
        public int IdUsuario { get; }

        /// <summary>
        /// Nome da conta
        /// </summary>
        public string Nome { get; }

        /// <summary>
        /// Valor inicial do saldo da conta
        /// </summary>
        public decimal? ValorSaldoInicial { get; set; }

        /// <summary>
        /// Nome da instituição financeira a qual a conta pertence
        /// </summary>
        public string NomeInstituicao { get; set; }

        /// <summary>
        /// Número da agência da conta
        /// </summary>
        public string NumeroAgencia { get; set; }

        /// <summary>
        /// Número da conta
        /// </summary>
        public string Numero { get; set; }

        public CadastrarContaEntrada(int idUsuario, string nome)
        {
            this.IdUsuario = idUsuario;
            this.Nome = nome;
        }

        public bool Valido()
        {
            this
                .NotificarSeMenorOuIgualA(this.IdUsuario, 0, $"O ID do usuário informado ({this.IdUsuario}) é inválido.")
                .NotificarSeNuloOuVazio(this.Nome, "O nome é obrigatório e não foi informado.");

            return !this.Invalido;
        }
    }
}
