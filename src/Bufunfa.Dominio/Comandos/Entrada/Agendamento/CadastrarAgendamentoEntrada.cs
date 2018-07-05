using JNogueira.Bufunfa.Dominio.Interfaces.Comandos;
using JNogueira.Bufunfa.Dominio.Resources;
using JNogueira.Infraestrutura.NotifiqueMe;

namespace JNogueira.Bufunfa.Dominio.Comandos.Entrada
{
    /// <summary>
    /// Comando utilizado para o cadastro de um novo agendamento
    /// </summary>
    public class CadastrarAgendamentoEntrada : Notificavel, IEntrada
    {
        /// <summary>
        /// Id do usuário proprietário da conta
        /// </summary>
        public int IdUsuario { get; }

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


        public CadastrarAgendamentoEntrada(int idUsuario, int idCategoria, int? idConta, int? idCartaoCredito, MetodoPagamento tipoMetodoPagamento, int? idPessoa = null, string observacao = null)
        {
            this.IdUsuario           = idUsuario;
            this.IdCategoria         = idCategoria;
            this.IdConta             = idConta;
            this.IdCartaoCredito     = idCartaoCredito;
            this.TipoMetodoPagamento = tipoMetodoPagamento;
            this.IdPessoa            = idPessoa;
            this.Observacao          = observacao;
        }

        public bool Valido()
        {
            this
                .NotificarSeMenorOuIgualA(this.IdUsuario, 0, string.Format(Mensagem.Id_Usuario_Invalido, this.IdUsuario))
                .NotificarSeMenorOuIgualA(this.IdCategoria, 0, string.Format(AgendamentoMensagem.Id_Categoria_Obrigatorio_Nao_Informado, this.IdCategoria))
                .NotificarSeVerdadeiro(!this.IdConta.HasValue && !this.IdCartaoCredito.HasValue, AgendamentoMensagem.Id_Conta_Id_Cartao_Credito_Nao_Informados)
                .NotificarSeVerdadeiro(this.IdConta.HasValue && this.IdCartaoCredito.HasValue, AgendamentoMensagem.Id_Conta_Id_Cartao_Credito_Informados);

            if (this.IdConta.HasValue)
                this.NotificarSeMenorQue(this.IdConta.Value, 1, string.Format(AgendamentoMensagem.Id_Conta_Invalido, this.IdConta.Value));

            if (this.IdCartaoCredito.HasValue)
                this.NotificarSeMenorQue(this.IdCartaoCredito.Value, 1, string.Format(AgendamentoMensagem.Id_Cartao_Credito_Invalido, this.IdCartaoCredito.Value));

            if (this.IdPessoa.HasValue)
                this.NotificarSeMenorQue(this.IdPessoa.Value, 1, string.Format(AgendamentoMensagem.Id_Pessoa_Invalido, this.IdPessoa.Value));

            if (!string.IsNullOrEmpty(this.Observacao))
                this.NotificarSePossuirTamanhoSuperiorA(this.Observacao, 500, AgendamentoMensagem.Observacao_Tamanho_Maximo_Excedido);

            return !this.Invalido;
        }
    }
}
