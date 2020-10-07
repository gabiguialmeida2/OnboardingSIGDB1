using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using OnboardingSIGDB1.Domain.Entitys;

namespace OnboardingSIGDB1.Data.Mappings
{
    public static class EmpresaMapping
    {
        public static void Map(this EntityTypeBuilder<Empresa> builder)
        {
            builder
                .ToTable("Empresa");

            builder
                .HasKey(p => p.Id);

            builder
                .Property(p => p.Cnpj)
                .IsRequired()
                .HasMaxLength(14);

            builder
                .Property(p => p.Nome)
                .IsRequired()
                .HasMaxLength(150);
        }
    }
}
