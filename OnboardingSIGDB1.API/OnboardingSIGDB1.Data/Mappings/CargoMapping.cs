using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnboardingSIGDB1.Domain.Entitys;

namespace OnboardingSIGDB1.Data.Mappings
{
    public static class CargoMapping
    {
        public static void Map(this EntityTypeBuilder<Cargo> builder)
        {
            builder
                .ToTable("Cargo");

            builder
                .HasKey(p => p.Id);

            builder
                .Property(p => p.Descricao)
                .IsRequired()
                .HasMaxLength(250);

        }
    }
}
