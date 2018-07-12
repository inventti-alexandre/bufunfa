using JNogueira.Bufunfa.Dominio.Interfaces.Comandos;
using JNogueira.Bufunfa.Dominio.Resources;
using JNogueira.Infraestrutura.NotifiqueMe;
using System;

namespace JNogueira.Bufunfa.Dominio.Comandos.Entrada
{
    /// <summary>
    /// Comando utilizado para o cadastro de uma nova parcela
    /// </summary>
    public class CadastrarParcelaEntrada : Notificavel, IEntrada
    {
        /// <summary>
        /// Id do usuário proprietário da conta
        /// </summary>
        public int IdUsuario { get; }

        /// <summary>
        /// Id da categoria
        /// </summary>
        public int? IdAgendamento { get; }

        /// <summary>
        /// Id da fatura
        /// </summary>
        public int? IdFatura { get; }

        /// <summary>
        /// Data da parcela
        /// </summary>
        public DateTime Data { get; }

        /// <summary>
        /// Valor de parcela
        /// </summary>
        public decimal Valor { get; }

        /// <summary>
        /// Observação da parcela
        /// </summary>
        public string Observacao { get; }

        public CadastrarParcelaEntrada(
            int idUsuario,
            int? idAgendamento,
            int? idFatura,
            DateTime data,
            decimal valor,
            string observacao = null)
        {
            this.IdUsuario     = idUsuario;
            this.IdAgendamento = idAgendamento;
            this.IdFatura      = idFatura;
            this.Data          = data;
            this.Valor         = valor;
            this.Observacao    = observacao;
        }

        public bool Valido()
        {
            this
                .NotificarSeMenorOuIgualA(this.IdUsuario, 0, string.Format(Mensagem.Id_Usuario_Invalido, this.IdUsuario))
                .NotificarSeVerdadeiro(!this.IdAgendamento.HasValue && !this.IdFatura.HasValue, ParcelaMensagem.Id_Agendamento_Id_Fatura_Nao_Informados)
                .NotificarSeVerdadeiro(this.IdAgendamento.HasValue && this.IdFatura.HasValue, ParcelaMensagem.Id_Agendamento_Id_Fatura_Informados);

            if (this.IdAgendamento.HasValue)
                this.NotificarSeMenorQue(this.IdAgendamento.Value, 1, string.Format(ParcelaMensagem.Id_Agendamento_Invalido, this.IdAgendamento.Value));

            if (this.IdFatura.HasValue)
                this.NotificarSeMenorQue(this.IdFatura.Value, 1, string.Format(ParcelaMensagem.Id_Fatura_Invalido, this.IdFatura.Value));

            if (!string.IsNullOrEmpty(this.Observacao))
                this.NotificarSePossuirTamanhoSuperiorA(this.Observacao, 500, ParcelaMensagem.Observacao_Tamanho_Maximo_Excedido);

            return !this.Invalido;
        }
    }
}
