using JNogueira.Bufunfa.Api.ViewModels;
using JNogueira.Bufunfa.Dominio;
using JNogueira.Bufunfa.Dominio.Comandos.Saida;
using JNogueira.Bufunfa.Dominio.Resources;
using Swashbuckle.AspNetCore.Filters;

namespace JNogueira.Bufunfa.Api.Swagger.Exemplos
{
    public class ProcurarCategoriaRequestExemplo : IExamplesProvider
    {
        public object GetExamples()
        {
            return new ProcurarCategoriaViewModel
            {
                Caminho = "DÉBITO » Alimentação » Restaurante",
                IdCategoriaPai = null,
                Nome = "Restaurante",
                Tipo = TipoCategoria.Debito
            };
        }
    }

    public class ProcurarCategoriaResponseExemplo : IExamplesProvider
    {
        public object GetExamples()
        {
            return new ProcurarSaida(new[] {
                        new
                        {
                            Id = 1,
                            Nome = "Restaurante",
                            Tipo = TipoCategoria.Debito,
                            Caminho = "DÉBITO » Alimentação » Restaurante",
                            CategoriaPai = new
                            {
                                Id = 1,
                                IdCategoriaPai = (int?) null,
                                Nome = "Restaurante",
                                Tipo = TipoCategoria.Debito,
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
                        }
                    }, "Nome", "ASC", 1, 1, 1, 1)
            {
                Mensagens = new[] { Mensagem.Procura_Resultado_Com_Sucesso }
            };
        }
    }

    public class CadastrarCategoriaRequestExemplo : IExamplesProvider
    {
        public object GetExamples()
        {
            return new CadastrarCategoriaViewModel
            {
                Nome = "Restaurante",
                Tipo = TipoCategoria.Debito,
                IdCategoriaPai = 1
            };
        }
    }

    public class CadastrarCategoriaResponseExemplo : IExamplesProvider
    {
        public object GetExamples()
        {
            return new Saida
            {
                Sucesso = true,
                Mensagens = new[] { CategoriaMensagem.Categoria_Cadastrada_Com_Sucesso },
                Retorno = new
                {
                    Id = 2,
                    Nome = "Restaurante",
                    Tipo = TipoCategoria.Debito,
                    IdCategoriaPai = 1
                }
            };
        }
    }

    public class AlterarCategoriaRequestExemplo : IExamplesProvider
    {
        public object GetExamples()
        {
            return new AlterarCategoriaViewModel
            {
                IdCategoria = 1,
                IdCategoriaPai = null,
                Nome = "Restaurante",
                Tipo = TipoCategoria.Debito
            };
        }
    }

    public class AlterarCategoriaResponseExemplo : IExamplesProvider
    {
        public object GetExamples()
        {
            return new Saida
            {
                Sucesso = true,
                Mensagens = new[] { CategoriaMensagem.Categoria_Alterada_Com_Sucesso },
                Retorno = new
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
                        Tipo = TipoCategoria.Debito,
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
                }
            };
        }
    }

    public class ExcluirCategoriaResponseExemplo : IExamplesProvider
    {
        public object GetExamples()
        {
            return new Saida
            {
                Sucesso = true,
                Mensagens = new[] { CategoriaMensagem.Categoria_Excluida_Com_Sucesso },
                Retorno = new
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
                        Tipo = TipoCategoria.Debito,
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
                }
            };
        }
    }

    public class ObterCategoriaPorIdResponseExemplo : IExamplesProvider
    {
        public object GetExamples()
        {
            return new Saida
            {
                Sucesso = true,
                Mensagens = new[] { CategoriaMensagem.Categoria_Encontrada_Com_Sucesso },
                Retorno = new
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
                        Tipo = TipoCategoria.Debito,
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
                }
            };
        }
    }

    public class ObterCategoriasPorUsuarioResponseExemplo : IExamplesProvider
    {
        public object GetExamples()
        {
            return new Saida
            {
                Sucesso = true,
                Mensagens = new[] { CategoriaMensagem.Categorias_Encontradas_Com_Sucesso },
                Retorno = new[]
                {
                    new
                    {
                        Id = 1,
                        Nome = "Restaurante",
                        Tipo = TipoCategoria.Debito,
                        Caminho = "DÉBITO » Alimentação » Restaurante",
                        CategoriaPai = new
                        {
                            Id = 1,
                            IdCategoriaPai = (int?) null,
                            Nome = "Restaurante",
                            Tipo = TipoCategoria.Debito,
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
                    }
                }
            };
        }
    }
}
