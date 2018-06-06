using JNogueira.Bufunfa.Dominio.Interfaces.Comandos;
using System.Collections.Generic;

namespace JNogueira.Bufunfa.Dominio.Comandos.Saida
{
    public class ComandoSaida : IComandoSaida
    {
        public ComandoSaida(bool sucesso, IEnumerable<string> mensagens, object retorno)
        {
            this.Sucesso = sucesso;
            this.Mensagens = mensagens;
            this.Retorno = retorno;
        }

        public bool Sucesso { get; set; }

        public IEnumerable<string> Mensagens { get; set; }

        public object Retorno { get; set; }
    }
}
