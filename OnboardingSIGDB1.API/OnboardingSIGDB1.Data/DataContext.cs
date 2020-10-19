using Microsoft.EntityFrameworkCore;
using OnboardingSIGDB1.Domain.Entitys;
using OnboardingSIGDB1.Data.Mappings;
using OnboardingSIGDB1.Domain.Empresas;
using OnboardingSIGDB1.Data.Empresas.Mappings;
using OnboardingSIGDB1.Domain.Cargos;
using OnboardingSIGDB1.Data.Cursos.Mapping;

namespace OnboardingSIGDB1.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        public DbSet<Empresa> Empresas { get; set; }
        public DbSet<Funcionario> Funcionarios { get; set; }
        public DbSet<Cargo> Cargos { get; set; }
        public DbSet<FuncionarioCargo> FuncionarioCargos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Empresa>().Map();
            modelBuilder.Entity<Funcionario>().Map();
            modelBuilder.Entity<Cargo>().Map();
            modelBuilder.Entity<FuncionarioCargo>().Map();
        }

    }
}
