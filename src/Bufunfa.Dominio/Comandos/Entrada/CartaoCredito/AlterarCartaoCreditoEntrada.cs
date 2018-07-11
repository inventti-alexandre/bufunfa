using JNogueira.Bufunfa.Dominio.Interfaces.Comandos;
using JNogueira.Bufunfa.Dominio.Resources;
using JNogueira.Infraestrutura.NotifiqueMe;

namespace JNogueira.Bufunfa.Dominio.Comandos.Entrada
{
    /// <summary>
    /// Comando utilizado para o alterar um cartão crédito
    /// </summary>
    public class AlterarCartaoCreditoEntrada : Notificavel, IEntrada
    {
        /// <summary>
        /// Id do usuário proprietário
        /// </summary>
        public int IdUsuario { get; }

        /// <summary>
        /// ID do cartão
        /// </summary>
        public int IdCartaoCredito { get; }

        /// <summary>
        /// Nome para identificação do cartão
        /// </summary>
        public string Nome { get; }

        /// <summary>
        /// Valor do limite do cartão
        /// </summary>
        public decimal ValorLimite { get; }

        /// <summary>
        /// Dia do vencimento da fatura do cartão
        /// </summary>
        public int DiaVencimentoFatura { get; }

        public AlterarCartaoCreditoEntrada(
            int idCartaoCredito,
            string nome,
            int idUsuario,
            decimal valorLimite,
            int diaVencimentoFatura)
        {
            this.IdCartaoCredito       = idCartaoCredito;
            this.IdUsuario             = idUsuario;
            this.Nome                  = nome;
            this.ValorLimite           = valorLimite;
            this.DiaVencimentoFatura   = diaVencimentoFatura;
        }

        public bool Valido()
        {
            this
                .NotificarSeMenorOuIgualA(this.IdCartaoCredito, 0, string.Format(CartaoCreditoMensagem.Id_Cartao_Invalido, this.IdCartaoCredito))
                .NotificarSeMenorOuIgualA(this.IdUsuario, 0, string.Format(Mensagem.Id_Usuario_Invalido, this.IdUsuario))
                .NotificarSeNuloOuVazio(this.Nome, CartaoCreditoMensagem.Nome_Obrigatorio_Nao_Informado)
                .NotificarSeMenorOuIgualA(this.ValorLimite, 0, CartaoCreditoMensagem.Valor_Limite_Invalido)
                .NotificarSeFalso(this.DiaVencimentoFatura >= 1 && this.DiaVencimentoFatura <= 31, CartaoCreditoMensagem.Dia_Vencimento_Fatura_Invalido);

            if (!string.IsNullOrEmpty(this.Nome))
                this.NotificarSePossuirTamanhoSuperiorA(this.Nome, 100, CartaoCreditoMensagem.Nome_Tamanho_Maximo_Excedido);

            return !this.Invalido;
        }
    }
}
