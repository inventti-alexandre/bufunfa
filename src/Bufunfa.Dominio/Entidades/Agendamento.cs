using System;

namespace JNogueira.Bufunfa.Dominio.Entidades
{
    /// <summary>
    /// Classe que representa um agendamento
    /// </summary>
    public class Agendamento
    {
        public int IdAgendamento { get; internal set; }

        public int IdUsuario { get; }

        public int IdConta { get; }

        public int IdPessoa { get; set; }

        public MetodoPagamento TipoMetodoPagamento { get; set; }

        public DateTime DataPrimeiraParcela { get; set; }

        public DateTime DataUltimaParcela { get; set; }

        public int QuantidadeParcelas { get; set; }

        public decimal ValorTotal { get; set; }

        public string Observacao { get; set; }
    }
}
