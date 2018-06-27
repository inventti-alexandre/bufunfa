using JNogueira.Bufunfa.Dominio.Interfaces.Comandos;
using JNogueira.Bufunfa.Dominio.Resources;
using JNogueira.Infraestrutura.NotifiqueMe;
using System;

namespace JNogueira.Bufunfa.Dominio.Comandos.Entrada
{
    public abstract class ProcurarEntrada<T> : Notificavel, IEntrada
    {
        public int IdUsuario { get; private set; }

        /// <summary>
        /// Página atual da listagem que exibirá o resultado da pesquisa
        /// </summary>
        public int? PaginaIndex { get; private set; }

        /// <summary>
        /// Quantidade de registros exibidos por página na listagem que exibirá o resultado da pesquisa
        /// </summary>
        public int? PaginaTamanho { get; private set; }

        /// <summary>
        /// Nome da propriedade que deverá ser utilizada para ordenação do resultado da pesquisa
        /// </summary>
        public string OrdenarPor { get; private set; }

        /// <summary>
        /// Sentido da ordenação do resultado da pesquisa: "ASC" para crescente / "DESC" para decrescente
        /// </summary>
        public string OrdenarSentido { get; private set; }

        /// <summary>
        /// Total de registros do resultado da pesquisa (independente das condições de paginação informadas)
        /// </summary>
        public int TotalRegistros { get; set; }

        public ProcurarEntrada(int idUsuario, string ordenarPor, string ordenarSentido, int? paginaIndex = null, int? paginaTamanho = null)
        {
            this.IdUsuario = idUsuario;
            this.OrdenarPor = ordenarPor;
            this.OrdenarSentido = !string.Equals(ordenarSentido, "ASC", StringComparison.OrdinalIgnoreCase) || !string.Equals(ordenarSentido, "DESC", StringComparison.OrdinalIgnoreCase)
                ? "ASC"
                : ordenarSentido;
            this.PaginaIndex = paginaIndex;
            this.PaginaTamanho = paginaTamanho;
        }

        public bool Paginar()
        {
            return this.PaginaIndex.HasValue && this.PaginaTamanho.HasValue;
        }

        public virtual bool Valido()
        {
            if (this.PaginaIndex.HasValue)
                this.NotificarSeMenorQue(this.PaginaIndex.Value, 0, string.Format(Mensagem.Paginacao_Pagina_Index_Invalido, this.PaginaIndex));

            if (this.PaginaTamanho.HasValue)
                this.NotificarSeMenorQue(this.PaginaTamanho.Value, 1, string.Format(Mensagem.Paginacao_Pagina_Tamanho_Invalido, this.PaginaTamanho));

            return !this.Invalido;
        }
    }
}
