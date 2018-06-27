using System;
using System.Linq.Expressions;

namespace JNogueira.Bufunfa.Dominio.Comandos.Entrada
{
    public abstract class ProcurarEntrada<T>
    {
        public int IdUsuario { get; private set; }

        /// <summary>
        /// Página atual da listagem que exibirá o resultado da pesquisa
        /// </summary>
        public int PaginaIndex { get; private set; }

        /// <summary>
        /// Quantidade de registros exibidos por página na listagem que exibirá o resultado da pesquisa
        /// </summary>
        public int PaginaTamanho { get; private set; }

        /// <summary>
        /// Nome da propriedade que deverá ser utilizada para ordenação do resultado da pesquisa
        /// </summary>
        public Expression<Func<T, object>> OrdenarPor { get; private set; }

        /// <summary>
        /// Sentido da ordenação do resultado da pesquisa: "ASC" para crescente / "DESC" para decrescente
        /// </summary>
        public string OrdenarSentido { get; private set; }

        /// <summary>
        /// Total de registros do resultado da pesquisa (independente das condições de paginação informadas)
        /// </summary>
        public int TotalRegistros { get; set; }

        public ProcurarEntrada(Expression<Func<T, object>> ordenarPor, string ordenarSentido)
        {
            this.PaginaIndex = -1;
            this.PaginaTamanho = int.MaxValue;
            this.OrdenarPor = ordenarPor;
            this.OrdenarSentido = ordenarSentido;
        }

        public ProcurarEntrada(int idUsuario, int paginaIndex, int paginaTamanho, Expression<Func<T, object>> ordenarPor, string ordenarSentido)
            : this(ordenarPor, ordenarSentido)
        {
            this.IdUsuario = idUsuario;
            this.PaginaIndex = paginaIndex;
            this.PaginaTamanho = paginaTamanho;
        }

        public bool Paginar()
        {
            return !(this.PaginaIndex == -1 && this.PaginaTamanho == int.MaxValue);
        }
    }
}
