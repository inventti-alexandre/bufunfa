using JNogueira.Bufunfa.Dominio.Comandos.Entrada;
using JNogueira.Infraestrutura.NotifiqueMe;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JNogueira.Bufunfa.Dominio.Interfaces.Infraestrutura
{
    public interface IGestaoAnexos
    {
        Task RealizarUploadAnexo(DateTime dataLancamento, CadastrarAnexoEntrada cadastroEntrada);

        IEnumerable<Notificacao> ObterNotificacoes();
    }
}
