using JNogueira.Bufunfa.Dominio.Comandos.Saida;
using JNogueira.Bufunfa.Dominio.Resources;
using Swashbuckle.AspNetCore.Examples;

namespace JNogueira.Bufunfa.Api.Swagger.Exemplos
{
    public class ProcurarPessoaResponseExemplo : IExamplesProvider
    {
        public object GetExamples()
        {
            return new ProcurarSaida(new[] {
                        new
                        {
                            Id = 1,
                            Nome = "Pessoa 1"
                        },
                        new
                        {
                            Id = 2,
                            Nome = "Pessoa 2"
                        },
                        new
                        {
                            Id = 3,
                            Nome = "Pessoa 3"
                        }
                    }, "Nome", "ASC", 3, 1, 1, 3)
            {
                Mensagens = new[] { Mensagem.Procura_Resultado_Com_Sucesso }
            };
        }
    }
}
