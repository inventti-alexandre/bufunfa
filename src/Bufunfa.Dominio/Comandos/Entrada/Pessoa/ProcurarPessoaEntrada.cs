using JNogueira.Bufunfa.Dominio.Entidades;
using JNogueira.Bufunfa.Dominio.Resources;
using JNogueira.Infraestrutura.NotifiqueMe;

namespace JNogueira.Bufunfa.Dominio.Comandos.Entrada
{
    /// <summary>
    /// Classe com opções de filtro para procura de pessoas
    /// </summary>
    public class ProcurarPessoaEntrada : ProcurarEntrada<Pessoa>
    {
        public string Nome { get; set; }

        public ProcurarPessoaEntrada(int idUsuario, string ordenarPor, string ordenarSentido, int? paginaIndex = null, int? paginaTamanho = null)
            : base(idUsuario, string.IsNullOrEmpty(ordenarPor) ? "Id" : ordenarPor, string.IsNullOrEmpty(ordenarSentido) ? "ASC" : ordenarSentido, paginaIndex, paginaTamanho)
        {
            
        }

        public override bool Valido()
        {
            base.Valido();

            this.NotificarSeNulo(typeof(Pessoa).GetProperty(this.OrdenarPor), string.Format(Mensagem.Paginacao_OrdernarPor_Propriedade_Nao_Existe, this.OrdenarPor));

            if (!string.IsNullOrEmpty(this.Nome))
                this.NotificarSePossuirTamanhoSuperiorA(this.Nome, 200, PessoaMensagem.Nome_Tamanho_Maximo_Excedido);

            return !this.Invalido;
        }
    }
}
