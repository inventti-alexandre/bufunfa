using JNogueira.Bufunfa.Api.ViewModels;
using JNogueira.Bufunfa.Dominio.Comandos.Saida;
using JNogueira.Bufunfa.Dominio.Resources;
using Swashbuckle.AspNetCore.Examples;

namespace JNogueira.Bufunfa.Api.Swagger.Exemplos
{
    public class CadastrarCartaoCreditoViewModelExemplo : IExamplesProvider
    {
        public object GetExamples()
        {
            return new CadastrarCartaoCreditoViewModel
            {
                Nome = "Cartão VISA 1",
                ValorLimite = (decimal?)5000.00,
                DiaVencimentoFatura = 5
            };
        }
    }

    public class AlterarCartaoCreditoViewModelExemplo : IExamplesProvider
    {
        public object GetExamples()
        {
            return new AlterarCartaoCreditoViewModel
            {
                IdCartao = 1,
                Nome = "Cartão VISA 1",
                ValorLimite = (decimal?)5000.00,
                DiaVencimentoFatura = 5
            };
        }
    }

    public class CadastrarCartaoCreditoResponseExemplo : IExamplesProvider
    {
        public object GetExamples()
        {
            return new Saida
            {
                Sucesso = true,
                Mensagens = new[] { CartaoCreditoMensagem.Cartao_Cadastrado_Com_Sucesso },
                Retorno = new
                {
                    Id = 1,
                    Nome = "Cartão VISA 1",
                    ValorLimite = (decimal?)5000.00,
                    DiaVencimentoFatura = 5
                }
            };
        }
    }

    public class AlterarCartaoCreditoResponseExemplo : IExamplesProvider
    {
        public object GetExamples()
        {
            return new Saida
            {
                Sucesso = true,
                Mensagens = new[] { CartaoCreditoMensagem.Cartao_Alterado_Com_Sucesso },
                Retorno = new
                {
                    Id = 1,
                    Nome = "Cartão VISA 1",
                    ValorLimite = (decimal?)5000.00,
                    DiaVencimentoFatura = 5
                }
            };
        }
    }

    public class ExcluirCartaoCreditoResponseExemplo : IExamplesProvider
    {
        public object GetExamples()
        {
            return new Saida
            {
                Sucesso = true,
                Mensagens = new[] { CartaoCreditoMensagem.Cartao_Excluido_Com_Sucesso },
                Retorno = new
                {
                    Id = 1,
                    Nome = "Cartão VISA 1",
                    ValorLimite = (decimal?)5000.00,
                    DiaVencimentoFatura = 5
                }
            };
        }
    }

    public class ObterCartaoCreditoPorIdResponseExemplo : IExamplesProvider
    {
        public object GetExamples()
        {
            return new Saida
            {
                Sucesso = true,
                Mensagens = new[] { CartaoCreditoMensagem.Cartoes_Encontrados_Com_Sucesso },
                Retorno = new
                {
                    Id = 1,
                    Nome = "Cartão VISA 1",
                    ValorLimite = (decimal?)5000.00,
                    DiaVencimentoFatura = 5
                }
            };
        }
    }

    public class ObterCartaoCreditosPorUsuarioResponseExemplo : IExamplesProvider
    {
        public object GetExamples()
        {
            return new Saida
            {
                Sucesso = true,
                Mensagens = new[] { CartaoCreditoMensagem.Cartoes_Encontrados_Com_Sucesso },
                Retorno = new[]
                {
                    new
                    {
                        Id = 1,
                        Nome = "Cartão VISA 1",
                        ValorLimite = (decimal?)5000.00,
                        DiaVencimentoFatura = 5
                    }
                }
            };
        }
    }
}
