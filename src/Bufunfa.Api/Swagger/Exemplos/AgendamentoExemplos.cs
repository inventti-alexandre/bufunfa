using JNogueira.Bufunfa.Api.ViewModels;
using JNogueira.Bufunfa.Dominio;
using JNogueira.Bufunfa.Dominio.Comandos.Saida;
using JNogueira.Bufunfa.Dominio.Resources;
using Swashbuckle.AspNetCore.Examples;
using System;

namespace JNogueira.Bufunfa.Api.Swagger.Exemplos
{
    public class CadastrarAgendamentoViewModelExemplo : IExamplesProvider
    {
        public object GetExamples()
        {
            return new CadastrarAgendamentoViewModel
            {
                DataPrimeiraParcela = DateTime.Now.Date,
                IdCartaoCredito = null,
                IdCategoria = 1,
                IdConta = 1,
                IdPessoa = null,
                Observacao = "TV a cabo",
                PeriodicidadeParcelas = Periodicidade.Mensal,
                QuantidadeParcelas = 12,
                ValorParcela = (decimal?)100.23 
            };
        }
    }

    public class CadastrarAgendamentoResponseExemplo : IExamplesProvider
    {
        public object GetExamples()
        {
            return new Saida
            {
                Sucesso = true,
                Mensagens = new[] { AgendamentoMensagem.Agendamento_Cadastrado_Com_Sucesso },
                Retorno = new
                {
                    Id = 1,
                    Nome = "Supermecado Carone"
                }
            };
        }
    }

    public class AlterarAgendamentoViewModelExemplo : IExamplesProvider
    {
        public object GetExamples()
        {
            return new AlterarAgendamentoViewModel
            {
                IdAgendamento = 1,
                DataPrimeiraParcela = DateTime.Now.Date,
                IdCartaoCredito = null,
                IdCategoria = 1,
                IdConta = 1,
                IdPessoa = null,
                Observacao = "TV a cabo",
                PeriodicidadeParcelas = Periodicidade.Mensal,
                QuantidadeParcelas = 12,
                ValorParcela = (decimal?)100.23
            };
        }
    }

    public class AlterarAgendamentoResponseExemplo : IExamplesProvider
    {
        public object GetExamples()
        {
            return new Saida
            {
                Sucesso = true,
                Mensagens = new[] { AgendamentoMensagem.Agendamento_Alterado_Com_Sucesso },
                Retorno = new
                {
                    Id = 1,
                    Nome = "Supermecado Carone"
                }
            };
        }
    }

    public class ExcluirAgendamentoResponseExemplo : IExamplesProvider
    {
        public object GetExamples()
        {
            return new Saida
            {
                Sucesso = true,
                Mensagens = new[] { AgendamentoMensagem.Agendamento_Excluido_Com_Sucesso },
                Retorno = new
                {
                    Id = 1,
                    Nome = "Supermecado Carone"
                }
            };
        }
    }

    public class ObterAgendamentoPorIdResponseExemplo : IExamplesProvider
    {
        public object GetExamples()
        {
            return new Saida
            {
                Sucesso = true,
                Mensagens = new[] { AgendamentoMensagem.Agendamento_Encontrado_Com_Sucesso },
                Retorno = new
                {
                    Id = 1,
                    Nome = "Supermecado Carone"
                }
            };
        }
    }

    public class ProcurarAgendamentoResponseExemplo : IExamplesProvider
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

    public class ObterAgendamentosPorUsuarioResponseExemplo : IExamplesProvider
    {
        public object GetExamples()
        {
            return new Saida
            {
                Sucesso = true,
                Mensagens = new[] { AgendamentoMensagem.Agendamentos_Encontrados_Com_Sucesso },
                Retorno = new[]
                {
                    new
                    {
                        Id = 1,
                        Nome = "Supermecado Carone"
                    },
                    new
                    {
                        Id = 2,
                        Nome = "Padaria Pão Chick"
                    }
                }
            };
        }
    }
}
