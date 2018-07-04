using JNogueira.Bufunfa.Api.ViewModels;
using JNogueira.Bufunfa.Dominio.Comandos.Saida;
using JNogueira.Bufunfa.Dominio.Resources;
using Swashbuckle.AspNetCore.Examples;
using System;

namespace JNogueira.Bufunfa.Api.Swagger.Exemplos
{
    public class ProcurarPeriodoResponseExemplo : IExamplesProvider
    {
        public object GetExamples()
        {
            return new ProcurarSaida(new[] {
                        new
                        {
                            Id = 1,
                            Nome = "Período 1",
                            DataInicio = DateTime.Now.Date,
                            DataFim = DateTime.Now.Date.AddHours(23).AddMinutes(59).AddSeconds(59)
                        }
                    }, "DataInicio", "ASC", 1, 1, 1, 1)
            {
                Mensagens = new[] { Mensagem.Procura_Resultado_Com_Sucesso }
            };
        }
    }

    public class CadastrarPeriodoViewModelExemplo : IExamplesProvider
    {
        public object GetExamples()
        {
            return new CadastrarPeriodoViewModel
            {
                Nome = "Junho 2018",
                DataInicio = new DateTime(2018, 6, 1),
                DataFim = new DateTime(2018, 6, 30)
            };
        }
    }

    public class AlterarPeriodoViewModelExemplo : IExamplesProvider
    {
        public object GetExamples()
        {
            return new AlterarPeriodoViewModel
            {
                IdPeriodo = 1,
                Nome = "Junho 2018",
                DataInicio = new DateTime(2018, 6, 1),
                DataFim = new DateTime(2018, 6, 30)
            };
        }
    }

    public class CadastrarPeriodoResponseExemplo : IExamplesProvider
    {
        public object GetExamples()
        {
            return new Saida
            {
                Sucesso = true,
                Mensagens = new[] { PeriodoMensagem.Periodo_Cadastrado_Com_Sucesso },
                Retorno = new
                {
                    Id = 1,
                    Nome = "Junho 2018",
                    DataInicio = new DateTime(2018, 6, 1),
                    DataFim = new DateTime(2018, 6, 30)
                }
            };
        }
    }

    public class AlterarPeriodoResponseExemplo : IExamplesProvider
    {
        public object GetExamples()
        {
            return new Saida
            {
                Sucesso = true,
                Mensagens = new[] { PeriodoMensagem.Periodo_Alterado_Com_Sucesso },
                Retorno = new
                {
                    Id = 1,
                    Nome = "Junho 2018",
                    DataInicio = new DateTime(2018, 6, 1),
                    DataFim = new DateTime(2018, 6, 30)
                }
            };
        }
    }

    public class ExcluirPeridoResponseExemplo : IExamplesProvider
    {
        public object GetExamples()
        {
            return new Saida
            {
                Sucesso = true,
                Mensagens = new[] { PeriodoMensagem.Periodo_Excluido_Com_Sucesso },
                Retorno = new
                {
                    Id = 1,
                    Nome = "Junho 2018",
                    DataInicio = new DateTime(2018, 6, 1),
                    DataFim = new DateTime(2018, 6, 30)
                }
            };
        }
    }

    public class ObterPeriodoPorIdResponseExemplo : IExamplesProvider
    {
        public object GetExamples()
        {
            return new Saida
            {
                Sucesso = true,
                Mensagens = new[] { PeriodoMensagem.Periodo_Encontrado_Com_Sucesso },
                Retorno = new
                {
                    Id = 1,
                    Nome = "Junho 2018",
                    DataInicio = new DateTime(2018, 6, 1),
                    DataFim = new DateTime(2018, 6, 30)
                }
            };
        }
    }

    public class ObterPeriodosPorUsuarioResponseExemplo : IExamplesProvider
    {
        public object GetExamples()
        {
            return new Saida
            {
                Sucesso = true,
                Mensagens = new[] { PeriodoMensagem.Periodos_Encontrados_Com_Sucesso },
                Retorno = new[]
                {
                    new
                    {
                        Id = 1,
                        Nome = "Junho 2018",
                        DataInicio = new DateTime(2018, 6, 1),
                        DataFim = new DateTime(2018, 6, 30)
                    }
                }
            };
        }
    }
}
