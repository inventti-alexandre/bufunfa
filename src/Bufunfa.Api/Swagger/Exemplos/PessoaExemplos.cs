using JNogueira.Bufunfa.Api.ViewModels;
using JNogueira.Bufunfa.Dominio.Comandos.Saida;
using JNogueira.Bufunfa.Dominio.Resources;
using Swashbuckle.AspNetCore.Filters;

namespace JNogueira.Bufunfa.Api.Swagger.Exemplos
{
    public class CadastrarPessoaRequestExemplo : IExamplesProvider
    {
        public object GetExamples()
        {
            return new CadastrarPessoaViewModel
            {
                Nome = "Supermecado Carone"
            };
        }
    }

    public class CadastrarPessoaResponseExemplo : IExamplesProvider
    {
        public object GetExamples()
        {
            return new Saida
            {
                Sucesso = true,
                Mensagens = new[] { PessoaMensagem.Pessoa_Cadastrada_Com_Sucesso },
                Retorno = new
                {
                    Id = 1,
                    Nome = "Supermecado Carone"
                }
            };
        }
    }

    public class AlterarPessoaRequestExemplo : IExamplesProvider
    {
        public object GetExamples()
        {
            return new AlterarPessoaViewModel
            {
                IdPessoa = 1,
                Nome = "Supermecado Carone"
            };
        }
    }

    public class AlterarPessoaResponseExemplo : IExamplesProvider
    {
        public object GetExamples()
        {
            return new Saida
            {
                Sucesso = true,
                Mensagens = new[] { PessoaMensagem.Pessoa_Alterada_Com_Sucesso },
                Retorno = new
                {
                    Id = 1,
                    Nome = "Supermecado Carone"
                }
            };
        }
    }

    public class ExcluirPessoaResponseExemplo : IExamplesProvider
    {
        public object GetExamples()
        {
            return new Saida
            {
                Sucesso = true,
                Mensagens = new[] { PessoaMensagem.Pessoa_Excluida_Com_Sucesso },
                Retorno = new
                {
                    Id = 1,
                    Nome = "Supermecado Carone"
                }
            };
        }
    }

    public class ObterPessoaPorIdResponseExemplo : IExamplesProvider
    {
        public object GetExamples()
        {
            return new Saida
            {
                Sucesso = true,
                Mensagens = new[] { PessoaMensagem.Pessoa_Encontrada_Com_Sucesso },
                Retorno = new
                {
                    Id = 1,
                    Nome = "Supermecado Carone"
                }
            };
        }
    }

    public class ProcurarPessoaRequestExemplo : IExamplesProvider
    {
        public object GetExamples()
        {
            return new ProcurarPessoaViewModel
            {
                Nome = "Fulano",
                OrdenarPor = "Nome",
                OrdenarSentido = "ASC",
                PaginaIndex = 1,
                PaginaTamanho = 50
            };
        }
    }

    public class ProcurarPessoaResponseExemplo : IExamplesProvider
    {
        public object GetExamples()
        {
            return new ProcurarSaida(new[] {
                        new
                        {
                            Id = 1,
                            Nome = "Supermecado Carone"
                        },
                        new
                        {
                            Id = 2,
                            Nome = "Padaria Pão Chick"
                        },
                        new
                        {
                            Id = 3,
                            Nome = "Farmácia Pague Menos"
                        }
                    }, "Nome", "ASC", 3, 1, 1, 3)
            {
                Mensagens = new[] { Mensagem.Procura_Resultado_Com_Sucesso }
            };
        }
    }
}
