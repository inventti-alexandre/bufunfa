using JNogueira.Bufunfa.Dominio.Resources;
using System.Collections.Generic;

namespace JNogueira.Bufunfa.Dominio.Comandos.Saida
{
    /// <summary>
    /// Comando para padronização das saídas de procuras realizadas
    /// </summary>
    public class ProcurarSaida : Saida
    {
        public ProcurarSaida(
            IEnumerable<object> registros,
            string ordenarPor,
            string ordenarSentido,
            int totalRegistros,
            int? totalPaginas = null,
            int? paginaIndex = null,
            int? paginaTamanho = null)
        {
            this.Sucesso = true;
            this.Mensagens = new[] { Mensagem.Procura_Resultado_Com_Sucesso };
            this.Retorno = new
            {
                PaginaIndex = paginaIndex,
                PaginaTamanho = paginaTamanho,
                OrdenarPor = ordenarPor,
                OrdenarSentido = ordenarSentido,
                TotalRegistros = totalRegistros,
                TotalPaginas = totalPaginas,
                Registros = registros
            };
        }
    }
}
