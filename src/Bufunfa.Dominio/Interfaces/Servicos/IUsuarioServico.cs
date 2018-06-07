using JNogueira.Bufunfa.Dominio.Comandos.Entrada;
using JNogueira.Bufunfa.Dominio.Interfaces.Comandos;

namespace JNogueira.Bufunfa.Dominio.Interfaces.Servicos
{
    public interface IUsuarioServico
    {
        IComandoSaida Autenticar(AutenticarUsuarioEntrada autenticacaoEntrada);

        IComandoSaida ObterUsuarioPorEmail(string email);
    }
}
