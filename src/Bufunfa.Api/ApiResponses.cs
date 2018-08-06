using JNogueira.Bufunfa.Dominio.Comandos.Saida;
using Swashbuckle.AspNetCore.Filters;
using System;

namespace JNogueira.Bufunfa.Api
{
    /// <summary>
    /// Response padrão da API para o erro HTTP 401
    /// </summary>
    public class UnauthorizedApiResponse : Saida, IExamplesProvider
    {
        public UnauthorizedApiResponse()
        {
            this.Sucesso = false;
            this.Mensagens = new[] { "Erro 401: Acesso negado. Certifique-se que você foi autenticado." };
            this.Retorno = null;
        }

        public object GetExamples()
        {
            return new UnauthorizedApiResponse();
        }
    }

    /// <summary>
    /// Response padrão da API para o erro HTTP 403
    /// </summary>
    public class ForbiddenApiResponse : Saida, IExamplesProvider
    {
        public ForbiddenApiResponse()
        {
            this.Sucesso = false;
            this.Mensagens = new[] { "Erro 403: Acesso negado. Você não tem permissão de acesso para essa funcionalidade." };
            this.Retorno = null;
        }

        public object GetExamples()
        {
            return new ForbiddenApiResponse();
        }
    }

    /// <summary>
    /// Response padrão da API para o erro HTTP 404
    /// </summary>
    public class NotFoundApiResponse : Saida, IExamplesProvider
    {
        public NotFoundApiResponse()
        {
            this.Sucesso = false;
            this.Mensagens = new[] { "Erro 404: O endereço não encontrado." };
            this.Retorno = null;
        }

        public NotFoundApiResponse(string path)
        {
            this.Sucesso = false;
            this.Mensagens = new[] { $"Erro 404: O endereço \"{path}\" não foi encontrado." };
            this.Retorno = null;
        }

        public object GetExamples()
        {
            return new NotFoundApiResponse();
        }
    }

    /// <summary>
    /// Response padrão da API para o erro HTTP 415
    /// </summary>
    public class UnsupportedMediaTypeApiResponse : Saida, IExamplesProvider
    {
        public UnsupportedMediaTypeApiResponse(string requestContentType)
        {
            this.Sucesso = false;
            this.Mensagens = new[] { $"Erro 415: O tipo de requisição \"{requestContentType}\" não é suportado pela API." };
            this.Retorno = null;
        }

        public object GetExamples()
        {
            return new ForbiddenApiResponse();
        }
    }

    /// <summary>
    /// Response padrão da API para o erro HTTP 500
    /// </summary>
    public class InternalServerErrorApiResponse : Saida, IExamplesProvider
    {
        public InternalServerErrorApiResponse()
        {

        }

        public InternalServerErrorApiResponse(Exception exception)
        {
            if (exception == null)
                return;

            this.Sucesso = false;
            this.Mensagens = new[] { exception.Message };
            this.Retorno = new
            {
                Exception = exception.Message,
                BaseException = exception.GetBaseException().Message,
                Source = exception.Source
            };
        }

        public object GetExamples()
        {
            try
            {
                var i = 1;

                try
                {
                    i /= 0;
                }
                catch (Exception ex1)
                {
                    throw new Exception("Ocorreu um erro inesperado.", ex1);
                }

                return null;
            }
            catch (Exception ex2)
            {
                return new InternalServerErrorApiResponse(ex2);
            }
        }
    }
}
