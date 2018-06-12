using JNogueira.Bufunfa.Api.ViewModels;
using JNogueira.Bufunfa.Dominio.Comandos.Saida;

namespace JNogueira.Bufunfa.Api.Swagger.Responses
{
    public class ResponseInternalServerError : ComandoSaida
    {
        public ResponseInternalServerError()
        {
            this.Sucesso = false;
            this.Mensagens = new[] { "Mensagem do erro encontrado." };
            this.Retorno = new ExceptionSaida
            {
                Exception = "Mensagem da execption",
                BaseException = "Mensagem base da exception",
                Source = "Source da expection"
            };
        }
    }
}
