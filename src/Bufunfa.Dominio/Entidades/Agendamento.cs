using JNogueira.Bufunfa.Dominio.Comandos.Entrada;
using System;
using System.Collections.Generic;
using System.Linq;

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

        /// <summary>
        /// Conta associada ao agendamento
        /// </summary>
        public Conta Conta { get; private set; }

        /// <summary>
        /// Cartão de crédito associado ao agendamento
        /// </summary>
        public CartaoCredito CartaoCredito { get; private set; }

        /// <summary>
        /// Pessoa associada ao agendamento
        /// </summary>
        public Pessoa Pessoa { get; private set; }

        /// <summary>
        /// Categoria do agendamento
        /// </summary>
        public Categoria Categoria { get; private set; }

        /// <summary>
        /// Parcelas do agendamento
        /// </summary>
        public IEnumerable<Parcela> Parcelas { get; private set; }

        /// <summary>
        /// Data de vencimento da próxima parcela aberta.
        /// </summary>
        public DateTime? DataProximaParcelaAberta
        {
            get
            {
                if (this.Parcelas == null || !this.Parcelas.Any())
                    return null;

                return this.Parcelas.Where(x => x.Status == StatusParcela.Aberta).Min(x => x.Data);
            }
        }

        /// <summary>
        /// Data de vencimento da última parcela aberta.
        /// </summary>
        public DateTime? DataUltimaParcelaAberta
        {
            get
            {
                if (this.Parcelas == null || !this.Parcelas.Any())
                    return null;

                return this.Parcelas.Where(x => x.Status == StatusParcela.Aberta).Max(x => x.Data);
            }
        }

        /// <summary>
        /// Quantidade total de parcelas.
        /// </summary>
        public int QuantidadeParcelas
        {
            get
            {
                if (this.Parcelas == null || !this.Parcelas.Any())
                    return 0;

                return this.Parcelas.Count();
            }
        }

        /// <summary>
        /// Quantidade total de parcelas abertas.
        /// </summary>
        public int QuantidadeParcelasAbertas
        {
            get
            {
                if (this.Parcelas == null || !this.Parcelas.Any())
                    return 0;

                return this.Parcelas.Count(x => x.Status == StatusParcela.Aberta);
            }
        }

        /// <summary>
        /// Quantidade total de parcelas fechadas.
        /// </summary>
        public int QuantidadeParcelasFechadas
        {
            get
            {
                if (this.Parcelas == null || !this.Parcelas.Any())
                    return 0;

                return this.Parcelas.Count(x => x.Status == StatusParcela.Fechada);
            }
        }

        /// <summary>
        /// Indica se o agendamento foi concluído, isto é, o número total de parcelas é igual ao número de parcelas descartadas e lançadas.
        /// </summary>
        public bool Concluido
        {
            get
            {
                if (this.Parcelas == null || !this.Parcelas.Any())
                    return true;

                return this.QuantidadeParcelas == this.QuantidadeParcelasFechadas;
            }
        }

        private Agendamento()
        {
            this.Parcelas = new List<Parcela>();
        }

        public Agendamento(CadastrarAgendamentoEntrada cadastrarEntrada)
            : this()
        {
            if (!cadastrarEntrada.Valido())
                return;

            this.IdUsuario           = cadastrarEntrada.IdUsuario;
            this.IdCategoria         = cadastrarEntrada.IdCategoria;
            this.IdConta             = cadastrarEntrada.IdConta;
            this.IdCartaoCredito     = cadastrarEntrada.IdCartaoCredito;
            this.IdPessoa            = cadastrarEntrada.IdPessoa;
            this.TipoMetodoPagamento = cadastrarEntrada.TipoMetodoPagamento;
            this.Observacao          = cadastrarEntrada.Observacao;
            this.Parcelas            = this.CriarParcelas(cadastrarEntrada.QuantidadeParcelas, cadastrarEntrada.DataPrimeiraParcela, cadastrarEntrada.ValorTotal, cadastrarEntrada.PeriodicidadeParcelas, cadastrarEntrada.Observacao);
        }

        public void Alterar(AlterarAgendamentoEntrada alterarEntrada)
        {
            if (!alterarEntrada.Valido() || alterarEntrada.IdAgendamento != this.Id)
                return;

            this.IdCategoria         = alterarEntrada.IdCategoria;
            this.IdConta             = alterarEntrada.IdConta;
            this.IdCartaoCredito     = alterarEntrada.IdCartaoCredito;
            this.IdPessoa            = alterarEntrada.IdPessoa;
            this.TipoMetodoPagamento = alterarEntrada.TipoMetodoPagamento;
            this.Observacao          = alterarEntrada.Observacao;
            this.Parcelas            = this.CriarParcelas(alterarEntrada.QuantidadeParcelas, alterarEntrada.DataPrimeiraParcela, alterarEntrada.ValorTotal, alterarEntrada.PeriodicidadeParcelas, alterarEntrada.Observacao);
        }

        public override string ToString()
        {
            var descricao = new List<string>();

            if (this.Conta != null)
                descricao.Add(this.Conta.Nome);

            if (this.CartaoCredito != null)
                descricao.Add(this.CartaoCredito.Nome);

            if (!string.IsNullOrEmpty(this.Observacao))
                descricao.Add(this.Observacao);

            return string.Join(" » ", descricao);
        }

        /// <summary>
        /// Cria as parcelas do agendamento
        /// </summary>
        /// <param name="quantidadeParcelas">Quantidade total de parcelas do agendamento</param>
        /// <param name="dataPrimeiraParcela">Data do vencimento da primeira parcela</param>
        /// <param name="valorTotal">Valor total do agendamento</param>
        /// <param name="periodicidade">Periodicidade no lançamento das parcelas</param>
        /// <param name="observacao">Observação da parcela</param>
        private IEnumerable<Parcela> CriarParcelas(int quantidadeParcelas, DateTime dataPrimeiraParcela, decimal valorTotal, Periodicidade periodicidade, string observacao)
        {
            var valorParcela = valorTotal / quantidadeParcelas;

            var parcela1 = new Parcela(new CadastrarParcelaEntrada(this.IdUsuario, this.Id, null, dataPrimeiraParcela, valorParcela, quantidadeParcelas > 1
                ? (!string.IsNullOrEmpty(observacao)
                    ? $"{observacao} / Parcela (1/{quantidadeParcelas})"
                    : $"Parcela (1/{quantidadeParcelas})")
                : observacao));

            var parcelas = new List<Parcela>() { parcela1 };

            var cont = 1;

            var dataParcela = dataPrimeiraParcela;

            while(cont < quantidadeParcelas)
            {
                cont++;

                dataParcela = dataParcela.AddMonths((int)periodicidade);

                var parcela = new Parcela(new CadastrarParcelaEntrada(this.IdUsuario, this.Id, null, dataParcela, valorParcela, !string.IsNullOrEmpty(observacao)
                    ? $"{observacao} / Parcela ({cont}/{quantidadeParcelas})"
                    : $"Parcela ({cont}/{quantidadeParcelas})"));

                parcelas.Add(parcela);
            }

            return parcelas;
        }
    }
}
