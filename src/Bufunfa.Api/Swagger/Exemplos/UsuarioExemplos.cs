﻿using JNogueira.Bufunfa.Dominio.Comandos.Saida;
using JNogueira.Bufunfa.Dominio.Resources;
using Swashbuckle.AspNetCore.Filters;
using System;

namespace JNogueira.Bufunfa.Api.Swagger.Exemplos
{
    public class AutenticarUsuarioResponseExemplo : IExamplesProvider
    {
        public object GetExamples()
        {
            return new Saida
            {
                Sucesso = true,
                Mensagens = new [] { UsuarioMensagem.Usuario_Autenticado_Com_Sucesso },
                Retorno = new
                {
                    DataCriacaoToken = DateTime.Now,
                    DataExpiracaoToken = DateTime.Now.AddHours(1),
                    Token = "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6WyJKT1JHRSBMVUlaIE5PR1VFSVJBIiwiamxucGluaGVpcm9AZ21haWwuY29tIl0sImp0aSI6Ijk3ZmU1OTk2ZmRjYzQ2ZDViZDU3MzVlNzRhZWE0Njk2IiwiY2FkYXN0cmFyLXVzdWFyaW8iOiJjYWRhc3RyYXItdXN1YXJpbyIsImNvbnN1bHRhci11c3VhcmlvIjoiY29uc3VsdGFyLXVzdWFyaW8iLCJuYmYiOjE1Mjg4OTkyNDMsImV4cCI6MTUyODkwMjg0MywiaWF0IjoxNTI4ODk5MjQzLCJpc3MiOiJCdWZ1bmZhSXNzdWVyIiwiYXVkIjoiQnVmdW5mYUF1ZGllbmNlIn0.VZijkyObZyg6dt89cW3eZ-6IkWZC1wfgf7M9w-G48OOhtPrxbsrVBxSf-2J79Vvaf35FQ_Zd4HV_LVpd-Eo4tXezUIDHP28CcuSZFjGIu-clAQFX-wplcYvgc_ODB7Z5YCGkmAXKbYFEVZ8wRTc1PwB4bh88SSkko3TLuCd6uG6GrTbDqLVULhW9AsV0nM0xZmXY1g4XvqmbGGcSdxwinNmWFhLRYNOcH7qL7-nfHesj3TJzgnsyeQde9K3hTASYMkln0kpuoT-7bAURBxWJ1TdgT-hENkIeTz0RwSGYQA5kzz9fFNm5VLZ20K9oehAmrBa05Z6ImE_wQnuxAqD1Kw",
                }
            };
        }
    }
}
