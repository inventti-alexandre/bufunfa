using JNogueira.Bufunfa.Dominio.Interfaces.Comandos;
using JNogueira.Bufunfa.Dominio.Resources;
using JNogueira.Infraestrutura.NotifiqueMe;
using System;

namespace JNogueira.Bufunfa.Dominio.Comandos.Entrada
{
    /// <summary>
    /// Comando utilizado para alterar um agendamento
    /// </summary>
    public class AlterarAgendamentoEntrada : Notificavel, IEntrada
    {
        /// <summary>
        /// Id do usuário proprietário da conta
        /// </summary>
        public int IdUsuario { get; }

        /// <summary>
        /// Id do agendamento
        /// </summary>
        public int IdAgendamento { get; }

        /// <summary>
        /// Id da categoria
        /// </summary>
        public int IdCategoria { get; }

        /// <summary>
        /// Id da conta
        /// </summary>
        public int? IdConta { get; }

        /// <summary>
        /// Id do cartão de crédito
        /// </summary>
        public int? IdCartaoCredito { get; }

        /// <summary>
        /// Id da pessoa
        /// </summary>
        public int? IdPessoa { get; }

        /// <summary>
        /// Tipo do método de pagamento das parcelas
        /// </summary>
        public MetodoPagamento TipoMetodoPagamento { get; }

        /// <summary>
        /// Observação sobre o agendamento
        /// </summary>
        public string Observacao { get; }

        /// <summary>
        /// Valor da parcela
        /// </summary>
        public decimal ValorParcela { get; }

        /// <summary>
        /// Data da primeira parcela do agendamento
        /// </summary>
        public DateTime DataPrimeiraParcela { get; }

        /// <summary>
        /// Quantidade de parcelas
        /// </summary>
        public int QuantidadeParcelas { get; }

        /// <summary>
        /// Periodicidade das parcelas
        /// </summary>
        public Periodicidade PeriodicidadeParcelas { get; }

        public AlterarAgendamentoEntrada(
            int idAgendamento,
            int idCategoria,
            int? idConta,
            int? idCartaoCredito,
            MetodoPagamento tipoMetodoPagamento,
            decimal valorParcela,
            DateTime dataPrimeiraParcela,
            int quantidadeParcelas,
            Periodicidade periodicidadeParcelas,
            int idUsuario,
            int? idPessoa = null,
            string observacao = null)
        {
            this.IdAgendamento         = idAgendamento;
            this.IdUsuario             = idUsuario;
            this.IdCategoria           = idCategoria;
            this.IdConta               = idConta;
            this.IdCartaoCredito       = idCartaoCredito;
            this.TipoMetodoPagamento   = tipoMetodoPagamento;
            this.IdPessoa              = idPessoa;
            this.Observacao            = observacao;
            this.ValorParcela          = valorParcela;
            this.DataPrimeiraParcela   = dataPrimeiraParcela.Date;
            this.QuantidadeParcelas    = quantidadeParcelas;
            this.PeriodicidadeParcelas = periodicidadeParcelas;

            this.Validar();
        }

        private void Validar()
        {
            this
                .NotificarSeMenorOuIgualA(this.IdUsuario, 0, string.Format(Mensagem.Id_Usuario_Invalido, this.IdUsuario))
                .NotificarSeMenorOuIgualA(this.IdAgendamento, 0, string.Format(AgendamentoMensagem.Id_Agendamento_Invalido, this.IdAgendamento))
                .NotificarSeMenorOuIgualA(this.IdCategoria, 0, string.Format(AgendamentoMensagem.Id_Categoria_Obrigatorio_Nao_Informado, this.IdCategoria))
                .NotificarSeVerdadeiro(!this.IdConta.HasValue && !this.IdCartaoCredito.HasValue, AgendamentoMensagem.Id_Conta_Id_Cartao_Credito_Nao_Informados)
                .NotificarSeVerdadeiro(this.IdConta.HasValue && this.IdCartaoCredito.HasValue, AgendamentoMensagem.Id_Conta_Id_Cartao_Credito_Informados)
                .NotificarSeMenorQue(this.DataPrimeiraParcela, DateTime.Now.Date, AgendamentoMensagem.Data_Primeira_Parcela_Menor_Data_Atual)
                .NotificarSeMenorQue(this.QuantidadeParcelas, 1, AgendamentoMensagem.Quantidade_Parcelas_Inválida);

            if (this.IdConta.HasValue)
                this.NotificarSeMenorQue(this.IdConta.Value, 1, string.Format(AgendamentoMensagem.Id_Conta_Invalido, this.IdConta.Value));

            if (this.IdCartaoCredito.HasValue)
                this.NotificarSeMenorQue(this.IdCartaoCredito.Value, 1, string.Format(AgendamentoMensagem.Id_Cartao_Credito_Invalido, this.IdCartaoCredito.Value));

            if (this.IdPessoa.HasValue)
                this.NotificarSeMenorQue(this.IdPessoa.Value, 1, string.Format(AgendamentoMensagem.Id_Pessoa_Invalido, this.IdPessoa.Value));

            if (!string.IsNullOrEmpty(this.Observacao))
                this.NotificarSePossuirTamanhoSuperiorA(this.Observacao, 500, AgendamentoMensagem.Observacao_Tamanho_Maximo_Excedido);
        }
    }
}
