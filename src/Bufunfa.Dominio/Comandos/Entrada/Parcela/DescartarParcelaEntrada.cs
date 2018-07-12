using JNogueira.Bufunfa.Dominio.Interfaces.Comandos;
using JNogueira.Bufunfa.Dominio.Resources;
using JNogueira.Infraestrutura.NotifiqueMe;

namespace JNogueira.Bufunfa.Dominio.Comandos.Entrada
{
    /// <summary>
    /// Comando utilizado para descartar uma parcela
    /// </summary>
    public class DescartarParcelaEntrada : Notificavel, IEntrada
    {
        /// <summary>
        /// Id do usuário proprietário da conta
        /// </summary>
        public int IdUsuario { get; }

        /// <summary>
        /// Id da parcela
        /// </summary>
        public int IdParcela { get; }

        /// <summary>
        /// Descrição do motivo do descarta da parcela
        /// </summary>
        public string MotivoDescarte { get; }

        public DescartarParcelaEntrada(
            int idParcela,
            int idUsuario,
            string motivoDescarte = null)
        {
            this.IdParcela      = idParcela;
            this.IdUsuario      = idUsuario;
            this.MotivoDescarte = motivoDescarte;
        }

        public bool Valido()
        {
            this
                .NotificarSeMenorOuIgualA(this.IdUsuario, 0, string.Format(Mensagem.Id_Usuario_Invalido, this.IdUsuario))
                .NotificarSeMenorOuIgualA(this.IdParcela, 0, string.Format(ParcelaMensagem.Id_Parcela_Invalido, this.IdParcela));

            if (!string.IsNullOrEmpty(this.MotivoDescarte))
                this.NotificarSePossuirTamanhoSuperiorA(this.MotivoDescarte, 500, ParcelaMensagem.Motivo_Descarte_Tamanho_Maximo_Excedido);

            return !this.Invalido;
        }
    }
}
