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
                    TipoMetodoPagamento = 2,
                    Observacao = "Energia - 2018",
                    Conta = new
                    {
                        Id = 1,
                        Nome = "Conta 1",
                        ValorSaldoInicial = 1000,
                        NomeInstituicao = (string)null,
                        NumeroAgencia = (string)null,
                        Numero = (string)null
                    },
                    CartaoCredito = (string)null,
                    Pessoa = new
                    {
                        Id = 80,
                        Nome = "EDP Escelsa"
                    },
                    Categoria = new
                    {
                        Id = 16,
                        Nome = "Energia elétrica",
                        Tipo = "D",
                        Caminho = "DÉBITO » Serviços » Energia elétrica",
                        CategoriaPai = new
                        {
                            Id = 6035,
                            IdCategoriaPai = (int?)null,
                            Nome = "Serviços",
                            Tipo = "D",
                            Caminho = "DÉBITO » Serviços"
                        },
                        CategoriasFilha = (object)null
                    },
                    Parcelas = new[]
                    {
                        new
                        {
                            Id = 37,
                            IdAgendamento = 5,
                            IdFatura = (int?)null,
                            Data = new DateTime(2018, 7, 30),
                            Valor = (decimal)128.35,
                            Lancada = false,
                            Descartada = false,
                            MotivoDescarte = (string)null,
                            Observacao = "Energia - 2018 / Parcela (1/2)"
                       },
                        new
                        {
                            Id = 38,
                            IdAgendamento = 5,
                            IdFatura = (int?)null,
                            Data = new DateTime(2018, 8, 30),
                            Valor = (decimal)128.35,
                            Lancada = false,
                            Descartada = false,
                            MotivoDescarte = (string)null,
                            Observacao = "Energia - 2018 / Parcela (2/2)"
                       }
                    },
                    DataProximaParcelaAberta = new DateTime(2018, 7, 30),
                    DataUltimaParcelaAberta = new DateTime(2018, 8, 30),
                    QuantidadeParcelas = 2,
                    QuantidadeParcelasAbertas = 2,
                    QuantidadeParcelasFechadas = 0,
                    Concluido = false
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
                    TipoMetodoPagamento = 2,
                    Observacao = "Energia - 2018",
                    Conta = new
                    {
                        Id = 1,
                        Nome = "Conta 1",
                        ValorSaldoInicial = 1000,
                        NomeInstituicao = (string)null,
                        NumeroAgencia = (string)null,
                        Numero = (string)null
                    },
                    CartaoCredito = (string)null,
                    Pessoa = new
                    {
                        Id = 80,
                        Nome = "EDP Escelsa"
                    },
                    Categoria = new
                    {
                        Id = 16,
                        Nome = "Energia elétrica",
                        Tipo = "D",
                        Caminho = "DÉBITO » Serviços » Energia elétrica",
                        CategoriaPai = new
                        {
                            Id = 6035,
                            IdCategoriaPai = (int?)null,
                            Nome = "Serviços",
                            Tipo = "D",
                            Caminho = "DÉBITO » Serviços"
                        },
                        CategoriasFilha = (object)null
                    },
                    Parcelas = new[]
                    {
                        new
                        {
                            Id = 37,
                            IdAgendamento = 5,
                            IdFatura = (int?)null,
                            Data = new DateTime(2018, 7, 30),
                            Valor = (decimal)128.35,
                            Lancada = false,
                            Descartada = false,
                            MotivoDescarte = (string)null,
                            Observacao = "Energia - 2018 / Parcela (1/2)"
                       },
                        new
                        {
                            Id = 38,
                            IdAgendamento = 5,
                            IdFatura = (int?)null,
                            Data = new DateTime(2018, 8, 30),
                            Valor = (decimal)128.35,
                            Lancada = false,
                            Descartada = false,
                            MotivoDescarte = (string)null,
                            Observacao = "Energia - 2018 / Parcela (2/2)"
                       }
                    },
                    DataProximaParcelaAberta = new DateTime(2018, 7, 30),
                    DataUltimaParcelaAberta = new DateTime(2018, 8, 30),
                    QuantidadeParcelas = 2,
                    QuantidadeParcelasAbertas = 2,
                    QuantidadeParcelasFechadas = 0,
                    Concluido = false
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
                    TipoMetodoPagamento = 2,
                    Observacao = "Energia - 2018",
                    Conta = new
                    {
                        Id = 1,
                        Nome = "Conta 1",
                        ValorSaldoInicial = 1000,
                        NomeInstituicao = (string)null,
                        NumeroAgencia = (string)null,
                        Numero = (string)null
                    },
                    CartaoCredito = (string)null,
                    Pessoa = new
                    {
                        Id = 80,
                        Nome = "EDP Escelsa"
                    },
                    Categoria = new
                    {
                        Id = 16,
                        Nome = "Energia elétrica",
                        Tipo = "D",
                        Caminho = "DÉBITO » Serviços » Energia elétrica",
                        CategoriaPai = new
                        {
                            Id = 6035,
                            IdCategoriaPai = (int?)null,
                            Nome = "Serviços",
                            Tipo = "D",
                            Caminho = "DÉBITO » Serviços"
                        },
                        CategoriasFilha = (object)null
                    },
                    Parcelas = new[]
                    {
                        new
                        {
                            Id = 37,
                            IdAgendamento = 5,
                            IdFatura = (int?)null,
                            Data = new DateTime(2018, 7, 30),
                            Valor = (decimal)128.35,
                            Lancada = false,
                            Descartada = false,
                            MotivoDescarte = (string)null,
                            Observacao = "Energia - 2018 / Parcela (1/2)"
                       },
                        new
                        {
                            Id = 38,
                            IdAgendamento = 5,
                            IdFatura = (int?)null,
                            Data = new DateTime(2018, 8, 30),
                            Valor = (decimal)128.35,
                            Lancada = false,
                            Descartada = false,
                            MotivoDescarte = (string)null,
                            Observacao = "Energia - 2018 / Parcela (2/2)"
                       }
                    },
                    DataProximaParcelaAberta = new DateTime(2018, 7, 30),
                    DataUltimaParcelaAberta = new DateTime(2018, 8, 30),
                    QuantidadeParcelas = 2,
                    QuantidadeParcelasAbertas = 2,
                    QuantidadeParcelasFechadas = 0,
                    Concluido = false
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
                    TipoMetodoPagamento = 2,
                    Observacao = "Energia - 2018",
                    Conta = new
                    {
                        Id = 1,
                        Nome = "Conta 1",
                        ValorSaldoInicial = 1000,
                        NomeInstituicao = (string)null,
                        NumeroAgencia = (string)null,
                        Numero = (string)null
                    },
                    CartaoCredito = (string)null,
                    Pessoa = new
                    {
                        Id = 80,
                        Nome = "EDP Escelsa"
                    },
                    Categoria = new
                    {
                        Id = 16,
                        Nome = "Energia elétrica",
                        Tipo = "D",
                        Caminho = "DÉBITO » Serviços » Energia elétrica",
                        CategoriaPai = new
                        {
                            Id = 6035,
                            IdCategoriaPai = (int?)null,
                            Nome = "Serviços",
                            Tipo = "D",
                            Caminho = "DÉBITO » Serviços"
                        },
                        CategoriasFilha = (object)null
                    },
                    Parcelas = new[]
                    {
                        new
                        {
                            Id = 37,
                            IdAgendamento = 5,
                            IdFatura = (int?)null,
                            Data = new DateTime(2018, 7, 30),
                            Valor = (decimal)128.35,
                            Lancada = false,
                            Descartada = false,
                            MotivoDescarte = (string)null,
                            Observacao = "Energia - 2018 / Parcela (1/2)"
                       },
                        new
                        {
                            Id = 38,
                            IdAgendamento = 5,
                            IdFatura = (int?)null,
                            Data = new DateTime(2018, 8, 30),
                            Valor = (decimal)128.35,
                            Lancada = false,
                            Descartada = false,
                            MotivoDescarte = (string)null,
                            Observacao = "Energia - 2018 / Parcela (2/2)"
                       }
                    },
                    DataProximaParcelaAberta = new DateTime(2018, 7, 30),
                    DataUltimaParcelaAberta = new DateTime(2018, 8, 30),
                    QuantidadeParcelas = 2,
                    QuantidadeParcelasAbertas = 2,
                    QuantidadeParcelasFechadas = 0,
                    Concluido = false
                }
            };
        }
    }

    public class ProcurarAgendamentoResponseExemplo : IExamplesProvider
    {
        public object GetExamples()
        {
            return new ProcurarSaida(new[]
                    {
                        new
                        {
                            Id = 1,
                            TipoMetodoPagamento = 2,
                            Observacao = "Energia - 2018",
                            Conta = new
                            {
                                Id = 1,
                                Nome = "Conta 1",
                                ValorSaldoInicial = 1000,
                                NomeInstituicao = (string)null,
                                NumeroAgencia = (string)null,
                                Numero = (string)null
                            },
                            CartaoCredito = (string)null,
                            Pessoa = new
                            {
                                Id = 80,
                                Nome = "EDP Escelsa"
                            },
                            Categoria = new
                            {
                                Id = 16,
                                Nome = "Energia elétrica",
                                Tipo = "D",
                                Caminho = "DÉBITO » Serviços » Energia elétrica",
                                CategoriaPai = new
                                {
                                    Id = 6035,
                                    IdCategoriaPai = (int?)null,
                                    Nome = "Serviços",
                                    Tipo = "D",
                                    Caminho = "DÉBITO » Serviços"
                                },
                                CategoriasFilha = (object)null
                            },
                            Parcelas = new[]
                            {
                                new
                                {
                                    Id = 37,
                                    IdAgendamento = 5,
                                    IdFatura = (int?)null,
                                    Data = new DateTime(2018, 7, 30),
                                    Valor = (decimal)128.35,
                                    Lancada = false,
                                    Descartada = false,
                                    MotivoDescarte = (string)null,
                                    Observacao = "Energia - 2018 / Parcela (1/2)"
                               },
                                new
                                {
                                    Id = 38,
                                    IdAgendamento = 5,
                                    IdFatura = (int?)null,
                                    Data = new DateTime(2018, 8, 30),
                                    Valor = (decimal)128.35,
                                    Lancada = false,
                                    Descartada = false,
                                    MotivoDescarte = (string)null,
                                    Observacao = "Energia - 2018 / Parcela (2/2)"
                               }
                            },
                            DataProximaParcelaAberta = new DateTime(2018, 7, 30),
                            DataUltimaParcelaAberta = new DateTime(2018, 8, 30),
                            QuantidadeParcelas = 2,
                            QuantidadeParcelasAbertas = 2,
                            QuantidadeParcelasFechadas = 0,
                            Concluido = false
                        },

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
                        TipoMetodoPagamento = 2,
                        Observacao = "Energia - 2018",
                        Conta = new
                        {
                            Id = 1,
                            Nome = "Conta 1",
                            ValorSaldoInicial = 1000,
                            NomeInstituicao = (string)null,
                            NumeroAgencia = (string)null,
                            Numero = (string)null
                        },
                        CartaoCredito = (string)null,
                        Pessoa = new
                        {
                            Id = 80,
                            Nome = "EDP Escelsa"
                        },
                        Categoria = new
                        {
                            Id = 16,
                            Nome = "Energia elétrica",
                            Tipo = "D",
                            Caminho = "DÉBITO » Serviços » Energia elétrica",
                            CategoriaPai = new
                            {
                                Id = 6035,
                                IdCategoriaPai = (int?)null,
                                Nome = "Serviços",
                                Tipo = "D",
                                Caminho = "DÉBITO » Serviços"
                            },
                            CategoriasFilha = (object)null
                        },
                        Parcelas = new[]
                        {
                            new
                            {
                                Id = 37,
                                IdAgendamento = 5,
                                IdFatura = (int?)null,
                                Data = new DateTime(2018, 7, 30),
                                Valor = (decimal)128.35,
                                Lancada = false,
                                Descartada = false,
                                MotivoDescarte = (string)null,
                                Observacao = "Energia - 2018 / Parcela (1/2)"
                            },
                            new
                            {
                                Id = 38,
                                IdAgendamento = 5,
                                IdFatura = (int?)null,
                                Data = new DateTime(2018, 8, 30),
                                Valor = (decimal)128.35,
                                Lancada = false,
                                Descartada = false,
                                MotivoDescarte = (string)null,
                                Observacao = "Energia - 2018 / Parcela (2/2)"
                            }
                        },
                        DataProximaParcelaAberta = new DateTime(2018, 7, 30),
                        DataUltimaParcelaAberta = new DateTime(2018, 8, 30),
                        QuantidadeParcelas = 2,
                        QuantidadeParcelasAbertas = 2,
                        QuantidadeParcelasFechadas = 0,
                        Concluido = false
                    }
                }
            };
        }
    }
}
