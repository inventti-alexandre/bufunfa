using JNogueira.Bufunfa.Dominio.Interfaces.Comandos;
using JNogueira.Bufunfa.Dominio.Resources;
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
        /// Tipo da conta
        /// </summary>
        public TipoConta Tipo { get; }

        /// <summary>
        /// Valor inicial do saldo da conta
        /// </summary>
        public decimal? ValorSaldoInicial { get; }

        /// <summary>
        /// Nome da instituição financeira a qual a conta pertence
        /// </summary>
        public string NomeInstituicao { get; }

        /// <summary>
        /// Número da agência da conta
        /// </summary>
        public string NumeroAgencia { get; }

        /// <summary>
        /// Número da conta
        /// </summary>
        public string Numero { get; }

        public AlterarContaEntrada(
            int idConta,
            string nome,
            TipoConta tipo,
            int idUsuario,
            decimal? valorSaldoInicial = null,
            string nomeInstituicao = null,
            string numeroAgencia = null,
            string numero = null)
        {
            this.IdConta           = idConta;
            this.IdUsuario         = idUsuario;
            this.Nome              = nome;
            this.Tipo              = tipo;
            this.ValorSaldoInicial = valorSaldoInicial;
            this.NomeInstituicao   = nomeInstituicao;
            this.NumeroAgencia     = numeroAgencia;
            this.Numero            = numero;
        }

        public bool Valido()
        {
            this
                .NotificarSeMenorOuIgualA(this.IdConta, 0, string.Format(ContaMensagem.Id_Conta_Invalido, this.IdConta))
                .NotificarSeMenorOuIgualA(this.IdUsuario, 0, string.Format(Mensagem.Id_Usuario_Invalido, this.IdUsuario))
                .NotificarSeNuloOuVazio(this.Nome, ContaMensagem.Nome_Obrigatorio_Nao_Informado);

            if (!string.IsNullOrEmpty(this.Nome))
                this.NotificarSePossuirTamanhoSuperiorA(this.Nome, 100, ContaMensagem.Nome_Tamanho_Maximo_Excedido);

            if (!string.IsNullOrEmpty(this.NomeInstituicao))
                this.NotificarSePossuirTamanhoSuperiorA(this.NomeInstituicao, 500, ContaMensagem.Nome_Instituicao_Tamanho_Maximo_Excedido);

            if (!string.IsNullOrEmpty(this.NumeroAgencia))
                this.NotificarSePossuirTamanhoSuperiorA(this.NumeroAgencia, 20, ContaMensagem.Nome_Instituicao_Tamanho_Maximo_Excedido);

            if (!string.IsNullOrEmpty(this.Numero))
                this.NotificarSePossuirTamanhoSuperiorA(this.Numero, 20, ContaMensagem.Nome_Instituicao_Tamanho_Maximo_Excedido);

            return !this.Invalido;
        }
    }
}
