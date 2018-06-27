using JNogueira.Bufunfa.Dominio.Entidades;

namespace JNogueira.Bufunfa.Dominio.Comandos.Entrada
{
    /// <summary>
    /// Classe com opções de filtro para busca de pessoas
    /// </summary>
    public class ProcurarPessoaEntrada : ProcurarEntrada<Pessoa>
    {
        public string Nome { get; set; }

        public ProcurarPessoaEntrada(int idUsuario, int paginaIndex, int paginaTamanho, string ordenarPor, string ordenarSentido)
            : base(idUsuario, paginaIndex, paginaTamanho, x => typeof(Pessoa).GetProperty(ordenarPor), ordenarSentido)
        {

        }
    }
}
