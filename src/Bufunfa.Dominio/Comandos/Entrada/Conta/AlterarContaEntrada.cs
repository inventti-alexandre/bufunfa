using JNogueira.Bufunfa.Dominio.Interfaces.Comandos;
using JNogueira.Infraestrutura.NotifiqueMe;

namespace JNogueira.Bufunfa.Dominio.Comandos.Entrada
{
    /// <summary>
    /// Comando utilizado para o alterar uma conta
    /// </summary>
    public class AlterarContaEntrada : Notificavel, IEntrada
    {
        /// <summary>
        /// Id do usuário proprietário
        /// </summary>
        public int IdUsuario { get; }

        /// <summary>
        /// ID da conta
        /// </summary>
        public int IdConta { get; }

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

        public AlterarContaEntrada(int idConta, string nome, int idUsuario)
        {
            this.IdConta = idConta;
            this.Nome = nome;
            this.IdUsuario = idUsuario;
        }

        public bool Valido()
        {
            this
                .NotificarSeMenorOuIgualA(this.IdConta, 0, $"O ID da conta informado ({this.IdConta}) é inválido.")
                .NotificarSeMenorOuIgualA(this.IdUsuario, 0, $"O ID do usuário informado ({this.IdUsuario}) é inválido.")
                .NotificarSeNuloOuVazio(this.Nome, "O nome é obrigatório e não foi informado.");

            return !this.Invalido;
        }
    }
}
