using JNogueira.Bufunfa.Dominio.Comandos.Entrada.Usuario;
using JNogueira.Bufunfa.Dominio.Interfaces.Comandos;

namespace JNogueira.Bufunfa.Dominio.Interfaces.Servicos
{
    public interface IUsuarioServico
    {
        IComandoSaida Autenticar(AutenticarUsuarioComando autenticacaoComando);
    }
}
