using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnboardingSIGDB1.Domain.Entitys;

namespace OnboardingSIGDB1.Data.Mappings
{
    public static class FuncionarioMapping
    {
        public static void Map(this EntityTypeBuilder<Funcionario> builder)
        {
            builder
                .ToTable("Funcionario");

            builder
                .HasKey(p => p.Id);

            builder
                .Property(p => p.Cpf)
                .IsRequired()
                .HasMaxLength(11);

            builder
                .Property(p => p.Nome)
                .IsRequired()
                .HasMaxLength(150);

            builder
                .HasOne(p => p.Empresa)
                .WithMany(e => e.Funcionarios)
                .HasForeignKey(p => p.EmpresaId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
