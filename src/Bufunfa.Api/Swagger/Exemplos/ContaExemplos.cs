using JNogueira.Bufunfa.Api.ViewModels;
using JNogueira.Bufunfa.Dominio.Comandos.Saida;
using Swashbuckle.AspNetCore.Examples;

namespace JNogueira.Bufunfa.Api.Swagger.Exemplos
{
    public class CadastrarContaViewModelExemplo : IExamplesProvider
    {
        public object GetExamples()
        {
            return new CadastrarContaViewModel
            {
                IdUsuario = 1,
                Nome = "Conta corrente Santander",
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
                Mensagens = new[] { "Conta \"Conta corrente Santander\" cadastrada com sucesso." },
                Retorno = new
                {
                    Id = 1,
                    IdUsuario = 1,
                    Nome = "Conta corrente Santander",
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
                Mensagens = new[] { "Conta alterada com sucesso." },
                Retorno = new
                {
                    Id = 1,
                    IdUsuario = 1,
                    Nome = "Conta corrente Santander",
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
                Mensagens = new[] { "Conta excluída com sucesso." },
                Retorno = new
                {
                    Id = 1,
                    IdUsuario = 1,
                    Nome = "Conta corrente Santander",
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
                Mensagens = new[] { "Conta encontrada com sucesso." },
                Retorno = new
                {
                    Id = 1,
                    IdUsuario = 1,
                    Nome = "Conta corrente Santander",
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
                Mensagens = new[] { "Contas do usuário encontradas com sucesso." },
                Retorno = new[]
                {
                    new
                    {
                        Id = 1,
                        IdUsuario = 1,
                        Nome = "Conta corrente Santander",
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
