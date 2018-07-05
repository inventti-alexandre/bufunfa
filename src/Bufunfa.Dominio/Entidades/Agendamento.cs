using System;

namespace JNogueira.Bufunfa.Dominio.Entidades
{
    /// <summary>
    /// Classe que representa um agendamento
    /// </summary>
    public class Agendamento
    {
        /// <summary>
        /// Id do agendamento
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Id do usuário proprietário
        /// </summary>
        public int IdUsuario { get; private set; }

        /// <summary>
        /// Id da categoria
        /// </summary>
        public int IdCategoria { get; private set; }

        /// <summary>
        /// Id da conta
        /// </summary>
        public int? IdConta { get; private set; }

        /// <summary>
        /// Id do cartão de crédito
        /// </summary>
        public int? IdCartaoCredito { get; private set; }
        
        /// <summary>
        /// Id da pessoa
        /// </summary>
        public int? IdPessoa { get; private set; }

        /// <summary>
        /// Tipo do método de pagamento
        /// </summary>
        public MetodoPagamento TipoMetodoPagamento { get; private set; }

        /// <summary>
        /// Observação sobre o agendamento
        /// </summary>
        public string Observacao { get; private set; }

        private Agendamento()
        {

        }

        public Agendamento()
        {

        }
    }
}
