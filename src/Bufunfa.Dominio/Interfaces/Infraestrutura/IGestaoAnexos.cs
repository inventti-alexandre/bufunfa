using JNogueira.Bufunfa.Dominio.Comandos.Entrada;
using System;
using System.Collections.Generic;
using System.Text;

namespace JNogueira.Bufunfa.Dominio.Interfaces.Infraestrutura
{
    public interface IGestaoAnexos
    {
        void RealizarUploadAnexo(DateTime dataLancamento, CadastrarAnexoEntrada cadastroEntrada);
    }
}
