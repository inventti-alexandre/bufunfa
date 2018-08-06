using JNogueira.Bufunfa.Api.ViewModels;
using JNogueira.Bufunfa.Dominio.Comandos.Saida;
using JNogueira.Bufunfa.Dominio.Resources;
using Swashbuckle.AspNetCore.Filters;

namespace JNogueira.Bufunfa.Api.Swagger.Exemplos
{
    public class CadastrarAnexoRequestExemplo : IExamplesProvider
    {
        public object GetExamples()
        {
            return new CadastrarAnexoViewModel
            {
                IdLancamento = 1,
                Descricao = "Comprovante de pagamento",
                NomeArquivo = "net_082018_comprovante"
            };
        }
    }

    public class CadastrarAnexoResponseExemplo : IExamplesProvider
    {
        public object GetExamples()
        {
            return new Saida
            {
                Sucesso = true,
                Mensagens = new[] { AnexoMensagem.Anexo_Cadastrado_Com_Sucesso },
                Retorno = new
                {
                    Id = 1,
                    IdLancamento = 1,
                    Descricao = "Comprovante de pagamento",
                    NomeArquivo = "net_082018_comprovante"
                }
            };
        }
    }

    //public class AlterarPessoaRequestExemplo : IExamplesProvider
    //{
    //    public object GetExamples()
    //    {
    //        return new AlterarPessoaViewModel
    //        {
    //            IdPessoa = 1,
    //            Nome = "Supermecado Carone"
    //        };
    //    }
    //}

    //public class AlterarPessoaResponseExemplo : IExamplesProvider
    //{
    //    public object GetExamples()
    //    {
    //        return new Saida
    //        {
    //            Sucesso = true,
    //            Mensagens = new[] { PessoaMensagem.Pessoa_Alterada_Com_Sucesso },
    //            Retorno = new
    //            {
    //                Id = 1,
    //                Nome = "Supermecado Carone"
    //            }
    //        };
    //    }
    //}

    //public class ExcluirPessoaResponseExemplo : IExamplesProvider
    //{
    //    public object GetExamples()
    //    {
    //        return new Saida
    //        {
    //            Sucesso = true,
    //            Mensagens = new[] { PessoaMensagem.Pessoa_Excluida_Com_Sucesso },
    //            Retorno = new
    //            {
    //                Id = 1,
    //                Nome = "Supermecado Carone"
    //            }
    //        };
    //    }
    //}
}
