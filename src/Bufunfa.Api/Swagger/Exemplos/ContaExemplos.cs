using JNogueira.Bufunfa.Api.ViewModels;
using JNogueira.Bufunfa.Dominio;
using JNogueira.Bufunfa.Dominio.Comandos.Saida;
using JNogueira.Bufunfa.Dominio.Resources;
using Swashbuckle.AspNetCore.Examples;

namespace JNogueira.Bufunfa.Api.Swagger.Exemplos
{
    public class CadastrarContaViewModelExemplo : IExamplesProvider
    {
        public object GetExamples()
        {
            return new CadastrarContaViewModel
            {
                Nome = "Conta corrente Santander",
                Tipo = TipoConta.ContaCorrente,
                ValorSaldoInicial = (decimal?)(-1542.12),
                NomeInstituicao = "Banco Santander S/A",
                NumeroAgencia = "3345",
                Numero = "01005539-0"
            };
        }
    }

    public class AlterarContaViewModelExemplo : IExamplesProvider
    {
        public object GetExamples()
        {
            return new AlterarContaViewModel
            {
                IdConta = 1,
                Nome = "Conta corrente Santander",
                Tipo = TipoConta.ContaCorrente,
                ValorSaldoInicial = (decimal?)(-1542.1),
                NomeInstituicao = "Banco Santander",
                NumeroAgencia = "3345",
                Numero = "01005539-0"
            };
        }
    }

    public class CadastrarContaResponseExemplo : IExamplesProvider
    {
        public object GetExamples()
        {
            return new Saida
            {
                Sucesso = true,
                Mensagens = new[] { ContaMensagem.Conta_Cadastrada_Com_Sucesso },
                Retorno = new
                {
                    Id = 1,
                    Nome = "Conta corrente Santander",
                    Tipo = (int)TipoConta.ContaCorrente,
                    ValorSaldoInicial = (decimal?)(-1542.12),
                    NomeInstituicao = "Banco Santander S/A",
                    NumeroAgencia = "3345",
                    Numero = "01005539-0"
                }
            };
        }
    }

    public class AlterarContaResponseExemplo : IExamplesProvider
    {
        public object GetExamples()
        {
            return new Saida
            {
                Sucesso = true,
                Mensagens = new[] { ContaMensagem.Conta_Alterada_Com_Sucesso },
                Retorno = new
                {
                    Id = 1,
                    Nome = "Conta corrente Santander",
                    Tipo = (int)TipoConta.ContaCorrente,
                    ValorSaldoInicial = (decimal?)(-1542.12),
                    NomeInstituicao = "Banco Santander S/A",
                    NumeroAgencia = "3345",
                    Numero = "01005539-0"
                }
            };
        }
    }

    public class ExcluirContaResponseExemplo : IExamplesProvider
    {
        public object GetExamples()
        {
            return new Saida
            {
                Sucesso = true,
                Mensagens = new[] { ContaMensagem.Conta_Excluida_Com_Sucesso },
                Retorno = new
                {
                    Id = 1,
                    Nome = "Conta corrente Santander",
                    Tipo = (int)TipoConta.ContaCorrente,
                    ValorSaldoInicial = (decimal?)(-1542.12),
                    NomeInstituicao = "Banco Santander S/A",
                    NumeroAgencia = "3345",
                    Numero = "01005539-0"
                }
            };
        }
    }

    public class ObterContaPorIdResponseExemplo : IExamplesProvider
    {
        public object GetExamples()
        {
            return new Saida
            {
                Sucesso = true,
                Mensagens = new[] { ContaMensagem.Conta_Encontrada_Com_Sucesso },
                Retorno = new
                {
                    Id = 1,
                    Nome = "Conta corrente Santander",
                    Tipo = (int)TipoConta.ContaCorrente,
                    ValorSaldoInicial = (decimal?)(-1542.12),
                    NomeInstituicao = "Banco Santander S/A",
                    NumeroAgencia = "3345",
                    Numero = "01005539-0"
                }
            };
        }
    }

    public class ObterContasPorUsuarioResponseExemplo : IExamplesProvider
    {
        public object GetExamples()
        {
            return new Saida
            {
                Sucesso = true,
                Mensagens = new[] { ContaMensagem.Contas_Encontradas_Com_Sucesso },
                Retorno = new[]
                {
                    new
                    {
                        Id = 1,
                        Nome = "Conta corrente Santander",
                        Tipo = (int)TipoConta.ContaCorrente,
                        ValorSaldoInicial = (decimal?)(-1542.12),
                        NomeInstituicao = "Banco Santander S/A",
                        NumeroAgencia = "3345",
                        Numero = "01005539-0"
                    }
                }
            };
        }
    }
}
