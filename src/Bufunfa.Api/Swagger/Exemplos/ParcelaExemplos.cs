using JNogueira.Bufunfa.Api.ViewModels;
using JNogueira.Bufunfa.Dominio.Comandos.Saida;
using JNogueira.Bufunfa.Dominio.Resources;
using Swashbuckle.AspNetCore.Examples;
using System;

namespace JNogueira.Bufunfa.Api.Swagger.Exemplos
{
    public class CadastrarParcelaViewModelExemplo : IExamplesProvider
    {
        public object GetExamples()
        {
            return new CadastrarParcelaViewModel
            {
                IdAgendamento = 1,
                Data = DateTime.Now.Date,
                Valor = (decimal)134.21,
                Observacao = "Observação da parcela"
            };
        }
    }

    public class LancarParcelaViewModelExemplo : IExamplesProvider
    {
        public object GetExamples()
        {
            return new LancarParcelaViewModel
            {
                IdParcela = 1,
                Data = DateTime.Today,
                Valor = (decimal?)120.23,
                Observacao = "Observação do lançamento da parcela"
            };
        }
    }

    public class DescartarParcelaViewModelExemplo : IExamplesProvider
    {
        public object GetExamples()
        {
            return new DescartarParcelaViewModel
            {
                IdParcela = 1,
                MotivoDescarte = "Descrição do motivo do descarte da parcela"
            };
        }
    }

    public class CadastrarParcelaResponseExemplo : IExamplesProvider
    {
        public object GetExamples()
        {
            return new Saida
            {
                Sucesso = true,
                Mensagens = new[] { ParcelaMensagem.Parcela_Cadastrada_Com_Sucesso },
                Retorno = new
                {
                    Id = 12,
                    IdAgendamento = 8,
                    IdFatura = (int?)null,
                    Data = DateTime.Now.Date,
                    Valor = (decimal)23.10,
                    Lancada = false,
                    Descartada = false,
                    MotivoDescarte = (string)null,
                    Observacao = "Parcela nova"
                }
            };
        }
    }

    public class LancarParcelaResponseExemplo : IExamplesProvider
    {
        public object GetExamples()
        {
            return new Saida
            {
                Sucesso = true,
                Mensagens = new[] { ParcelaMensagem.Parcela_Lancada_Com_Sucesso },
                Retorno = new
                {
                    Id = 12,
                    IdAgendamento = 8,
                    IdFatura = (int?)null,
                    Data = DateTime.Now.Date,
                    Valor = (decimal)23.10,
                    Lancada = false,
                    Descartada = false,
                    MotivoDescarte = (string)null,
                    Observacao = "Observação da parcela"
                }
            };
        }
    }

    public class DescartarParcelaResponseExemplo : IExamplesProvider
    {
        public object GetExamples()
        {
            return new Saida
            {
                Sucesso = true,
                Mensagens = new[] { ParcelaMensagem.Parcela_Descartada_Com_Sucesso },
                Retorno = new
                {
                    Id = 12,
                    IdAgendamento = 8,
                    IdFatura = (int?)null,
                    Data = DateTime.Now.Date,
                    Valor = (decimal)23.10,
                    Lancada = false,
                    Descartada = false,
                    MotivoDescarte = (string)null,
                    Observacao = "Observação da parcela"
                }
            };
        }
    }

    public class AlterarParcelaViewModelExemplo : IExamplesProvider
    {
        public object GetExamples()
        {
            return new AlterarParcelaViewModel
            {
                IdParcela = 1,
                Data = DateTime.Now.Date,
                Valor = (decimal)134.21,
                Observacao = "Observação da parcela"
            };
        }
    }

    public class AlterarParcelaResponseExemplo : IExamplesProvider
    {
        public object GetExamples()
        {
            return new Saida
            {
                Sucesso = true,
                Mensagens = new[] { ParcelaMensagem.Parcela_Alterada_Com_Sucesso },
                Retorno = new
                {
                    Id = 12,
                    IdAgendamento = 8,
                    IdFatura = (int?)null,
                    Data = DateTime.Now.Date,
                    Valor = (decimal)23.10,
                    Lancada = false,
                    Descartada = false,
                    MotivoDescarte = (string)null,
                    Observacao = "Observação da parcela"
                }
            };
        }
    }

    public class ExcluirParcelaResponseExemplo : IExamplesProvider
    {
        public object GetExamples()
        {
            return new Saida
            {
                Sucesso = true,
                Mensagens = new[] { ParcelaMensagem.Parcela_Excluida_Com_Sucesso },
                Retorno = new
                {
                    Id = 12,
                    IdAgendamento = 8,
                    IdFatura = (int?)null,
                    Data = DateTime.Now.Date,
                    Valor = (decimal)23.10,
                    Lancada = false,
                    Descartada = false,
                    MotivoDescarte = (string)null,
                    Observacao = "Observação da parcela"
                }
            };
        }
    }

    public class ObterParcelaPorIdResponseExemplo : IExamplesProvider
    {
        public object GetExamples()
        {
            return new Saida
            {
                Sucesso = true,
                Mensagens = new[] { ParcelaMensagem.Parcela_Encontrada_Com_Sucesso },
                Retorno = new
                {
                    Id = 12,
                    IdAgendamento = 8,
                    IdFatura = (int?)null,
                    Data = DateTime.Now.Date,
                    Valor = (decimal)23.10,
                    Lancada = false,
                    Descartada = false,
                    MotivoDescarte = (string)null,
                    Observacao = "Observação da parcela"
                }
            };
        }
    }

    public class ProcurarParcelaResponseExemplo : IExamplesProvider
    {
        public object GetExamples()
        {
            return new ProcurarSaida(new[]
                    {
                        new
                        {
                            Id = 12,
                            IdAgendamento = 8,
                            IdFatura = (int?)null,
                            Data = DateTime.Now.Date,
                            Valor = (decimal)23.10,
                            Lancada = false,
                            Descartada = false,
                            MotivoDescarte = (string)null,
                            Observacao = "Observação da parcela"
                        },

                    }, "Data", "ASC", 3, 1, 1, 3)
            {
                Mensagens = new[] { Mensagem.Procura_Resultado_Com_Sucesso }
            };
        }
    }
}
