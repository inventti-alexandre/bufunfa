namespace JNogueira.Bufunfa.Dominio.Interfaces.Dados
{
    /// <summary>
    /// Contrato para utilização do padrão Unit Of Work
    /// </summary>
    public interface IUow
    {
        void Commit();
    }
}
