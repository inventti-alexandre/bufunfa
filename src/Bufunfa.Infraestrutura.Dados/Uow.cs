using JNogueira.Bufunfa.Dominio.Interfaces.Dados;

namespace JNogueira.Bufunfa.Infraestrutura.Dados
{
    /// <summary>
    /// Implementação do padrão Unit Of Work
    /// </summary>
    public class Uow : IUow
    {
        private readonly EfDataContext _context;

        public Uow(EfDataContext context)
        {
            _context = context;
        }

        public void Commit()
        {
            _context.SaveChanges();
        }
    }
}
