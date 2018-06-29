using JNogueira.Bufunfa.Dominio.Entidades;
using JNogueira.Bufunfa.Infraestrutura.Dados.Maps;
using Microsoft.EntityFrameworkCore;

namespace JNogueira.Bufunfa.Infraestrutura.Dados
{
    public class EfDataContext : DbContext
    {
        private readonly string _connectionString;

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Conta> Contas { get; set; }
        public DbSet<Periodo> Periodos { get; set; }
        public DbSet<Pessoa> Pessoas { get; set; }
        public DbSet<Categoria> Categorias { get; set; }

        public EfDataContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Mapeamentos para utilização do Entity Framework
            modelBuilder.ApplyConfiguration(new UsuarioMap());
            modelBuilder.ApplyConfiguration(new ContaMap());
            modelBuilder.ApplyConfiguration(new PeriodoMap());
            modelBuilder.ApplyConfiguration(new PessoaMap());
            modelBuilder.ApplyConfiguration(new CategoriaMap());
        }
    }
}
