using JNogueira.Bufunfa.Api.ViewModels;
using JNogueira.Bufunfa.Dominio;
using JNogueira.Bufunfa.Dominio.Comandos.Saida;
using JNogueira.Bufunfa.Dominio.Resources;
using Swashbuckle.AspNetCore.Filters;
using System;

namespace JNogueira.Bufunfa.Api.Swagger.Exemplos
{
    public class CadastrarLancamentoRequestExemplo : IExamplesProvider
    {
        public object GetExamples()
        {
            return new CadastrarLancamentoViewModel
            {
                Data = DateTime.Now,
                IdCategoria = 1,
                IdConta = 1,
                IdPessoa = 30,
                Observacao = "Observação do lançamento",
                Valor = (decimal)23.34
            };
        }
    }

    public class CadastrarLancamentoResponseExemplo : IExamplesProvider
    {
        public object GetExamples()
        {
            return new Saida
            {
                Sucesso = true,
                Mensagens = new[] { LancamentoMensagem.Lancamento_Cadastrado_Com_Sucesso },
                Retorno = new
                {
                    Id = 4,
                    IdParcela = (decimal?)null,
                    Data = DateTime.Now,
                    Valor = (decimal)23.34,
                    Observacao = "Observação",
                    Conta = new
                    {
                        Id = 1,
                        Nome = "Conta corrente Santander",
                        Tipo = (int)TipoConta.ContaCorrente,
                        ValorSaldoInicial = (decimal?)(-1542.12),
                        NomeInstituicao = "Banco Santander S/A",
                        NumeroAgencia = "3345",
                        Numero = "01005539-0"
                    },
                    Pessoa = (object)null,
                    Categoria = new
                    {
                        Id = 1,
                        Nome = "Restaurante",
                        Tipo = TipoCategoria.Debito,
                        Caminho = "DÉBITO » Alimentação » Restaurante",
                        CategoriaPai = new
                        {
                            Id = 1,
                            IdCategoriaPai = (int?)null,
                            Nome = "Restaurante",
                            Tipo = TipoCategoria.Debito
                        },
                        CategoriasFilha = new object[]
                        {
                            new
                            {
                                Id = 4,
                                Nome = "Selfservice",
                                Tipo = TipoCategoria.Debito,
                                Caminho = "DÉBITO » Alimentação » Restaurante » Selfservice",
                                CategoriasFilha = new object[] {}
                            }
                        }
                    },
                    Anexos = (object)null
                }
            };
        }
    }

    public class AlterarLancamentoRequestExemplo : IExamplesProvider
    {
        public object GetExamples()
        {
            return new AlterarLancamentoViewModel
            {
                IdLancamento = 2,
                Data = DateTime.Now,
                IdCategoria = 1,
                IdConta = 1,
                IdPessoa = 30,
                Observacao = "Observação do lançamento",
                Valor = (decimal)23.34
            };
        }
    }

    public class AlterarLancamentoResponseExemplo : IExamplesProvider
    {
        public object GetExamples()
        {
            return new Saida
            {
                Sucesso = true,
                Mensagens = new[] { LancamentoMensagem.Lancamento_Alterado_Com_Sucesso },
                Retorno = new
                {
                    Id = 4,
                    IdParcela = (decimal?)null,
                    Data = DateTime.Now,
                    Valor = (decimal)23.34,
                    Observacao = "Observação",
                    Conta = new
                    {
                        Id = 1,
                        Nome = "Conta corrente Santander",
                        Tipo = (int)TipoConta.ContaCorrente,
                        ValorSaldoInicial = (decimal?)(-1542.12),
                        NomeInstituicao = "Banco Santander S/A",
                        NumeroAgencia = "3345",
                        Numero = "01005539-0"
                    },
                    Pessoa = (object)null,
                    Categoria = new
                    {
                        Id = 1,
                        Nome = "Restaurante",
                        Tipo = TipoCategoria.Debito,
                        Caminho = "DÉBITO » Alimentação » Restaurante",
                        CategoriaPai = new
                        {
                            Id = 1,
                            IdCategoriaPai = (int?)null,
                            Nome = "Restaurante",
                            Tipo = TipoCategoria.Debito
                        },
                        CategoriasFilha = new object[]
                        {
                            new
                            {
                                Id = 4,
                                Nome = "Selfservice",
                                Tipo = TipoCategoria.Debito,
                                Caminho = "DÉBITO » Alimentação » Restaurante » Selfservice",
                                CategoriasFilha = new object[] {}
                            }
                        }
                    },
                    Anexos = (object)null
                }
            };
        }
    }

    public class ExcluirLancamentoResponseExemplo : IExamplesProvider
    {
        public object GetExamples()
        {
            return new Saida
            {
                Sucesso = true,
                Mensagens = new[] { LancamentoMensagem.Lancamento_Excluido_Com_Sucesso },
                Retorno = new
                {
                    Id = 1,
                    Nome = "Supermecado Carone"
                }
            };
        }
    }

    public class ObterLancamentoPorIdResponseExemplo : IExamplesProvider
    {
        public object GetExamples()
        {
            return new Saida
            {
                Sucesso = true,
                Mensagens = new[] { LancamentoMensagem.Lancamento_Encontrado_Com_Sucesso },
                Retorno = new
                {
                    Id = 4,
                    IdParcela = (decimal?)null,
                    Data = DateTime.Now,
                    Valor = (decimal)23.34,
                    Observacao = "Observação",
                    Conta = new
                    {
                        Id = 1,
                        Nome = "Conta corrente Santander",
                        Tipo = (int)TipoConta.ContaCorrente,
                        ValorSaldoInicial = (decimal?)(-1542.12),
                        NomeInstituicao = "Banco Santander S/A",
                        NumeroAgencia = "3345",
                        Numero = "01005539-0"
                    },
                    Pessoa = (object)null,
                    Categoria = new
                    {
                        Id = 1,
                        Nome = "Restaurante",
                        Tipo = TipoCategoria.Debito,
                        Caminho = "DÉBITO » Alimentação » Restaurante",
                        CategoriaPai = new
                        {
                            Id = 1,
                            IdCategoriaPai = (int?)null,
                            Nome = "Restaurante",
                            Tipo = TipoCategoria.Debito
                        },
                        CategoriasFilha = new object[]
                        {
                            new
                            {
                                Id = 4,
                                Nome = "Selfservice",
                                Tipo = TipoCategoria.Debito,
                                Caminho = "DÉBITO » Alimentação » Restaurante » Selfservice",
                                CategoriasFilha = new object[] {}
                            }
                        }
                    },
                    Anexos = (object)null
                }
            };
        }
    }

    public class ProcurarLancamentoRequestExemplo : IExamplesProvider
    {
        public object GetExamples()
        {
            return new ProcurarLancamentoViewModel
            {
                DataInicio = DateTime.Now.Date,
                DataFim = DateTime.Now.AddDays(5).Date,
                IdCategoria = 80,
                IdConta = 1,
                IdPessoa = null,
                OrdenarPor = "Data",
                OrdenarSentido = "DESC",
                PaginaIndex = 1,
                PaginaTamanho = 50
            };
        }
    }

    public class ProcurarLancamentoResponseExemplo : IExamplesProvider
    {
        public object GetExamples()
        {
            return new ProcurarSaida(new[] {
                        new
                        {
                            Id = 4,
                            IdParcela = (decimal?)null,
                            Data = DateTime.Now,
                            Valor = (decimal)23.34,
                            Observacao = "Observação",
                            Conta = new
                            {
                                Id = 1,
                                Nome = "Conta corrente Santander",
                                Tipo = (int)TipoConta.ContaCorrente,
                                ValorSaldoInicial = (decimal?)(-1542.12),
                                NomeInstituicao = "Banco Santander S/A",
                                NumeroAgencia = "3345",
                                Numero = "01005539-0"
                            },
                            Pessoa = (object)null,
                            Categoria = new
                            {
                                Id = 1,
                                Nome = "Restaurante",
                                Tipo = TipoCategoria.Debito,
                                Caminho = "DÉBITO » Alimentação » Restaurante",
                                CategoriaPai = new
                                {
                                    Id = 1,
                                    IdCategoriaPai = (int?)null,
                                    Nome = "Restaurante",
                                    Tipo = TipoCategoria.Debito
                                },
                                CategoriasFilha = new object[]
                                {
                                    new
                                    {
                                        Id = 4,
                                        Nome = "Selfservice",
                                        Tipo = TipoCategoria.Debito,
                                        Caminho = "DÉBITO » Alimentação » Restaurante » Selfservice",
                                        CategoriasFilha = new object[] {}
                                    }
                                }
                            },
                            Anexos = (object)null
                        }
                    }, "Nome", "ASC", 3, 1, 1, 3)
            {
                Mensagens = new[] { Mensagem.Procura_Resultado_Com_Sucesso }
            };
        }
    }
}
