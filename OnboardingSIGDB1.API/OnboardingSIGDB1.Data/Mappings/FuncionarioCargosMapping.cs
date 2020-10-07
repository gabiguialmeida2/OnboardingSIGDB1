using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnboardingSIGDB1.Domain.Entitys;

namespace OnboardingSIGDB1.Data.Mappings
{
    public static class FuncionarioCargosMapping
    {
        public static void Map(this EntityTypeBuilder<FuncionarioCargo> builder)
        {
            builder
                .ToTable("FuncionarioCargo");

            builder
                .HasKey(p => new { p.FuncionarioId, p.CargoId});

            builder
                .HasOne(p => p.Funcionario)
                .WithMany(fc => fc.FuncionarioCargos)
                .HasForeignKey(p => p.FuncionarioId)
                .OnDelete(DeleteBehavior.Restrict);


            builder
                .HasOne(p => p.Cargo)
                .WithMany(fc => fc.FuncionarioCargos)
                .HasForeignKey(p => p.CargoId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Property(p => p.DataVinculacao)
                .IsRequired();

        }
    }
}
