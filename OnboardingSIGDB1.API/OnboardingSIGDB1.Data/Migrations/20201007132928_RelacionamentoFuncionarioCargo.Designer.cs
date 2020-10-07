﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OnboardingSIGDB1.Data;

namespace OnboardingSIGDB1.Data.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20201007132928_RelacionamentoFuncionarioCargo")]
    partial class RelacionamentoFuncionarioCargo
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("OnboardingSIGDB1.Domain.Entitys.Cargo", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasMaxLength(250);

                    b.HasKey("Id");

                    b.ToTable("Cargo");
                });

            modelBuilder.Entity("OnboardingSIGDB1.Domain.Entitys.Empresa", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Cnpj")
                        .IsRequired()
                        .HasMaxLength(14);

                    b.Property<DateTime?>("DataFundacao");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(150);

                    b.HasKey("Id");

                    b.ToTable("Empresa");
                });

            modelBuilder.Entity("OnboardingSIGDB1.Domain.Entitys.Funcionario", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Cpf")
                        .IsRequired()
                        .HasMaxLength(11);

                    b.Property<DateTime?>("DataContratacao");

                    b.Property<long?>("EmpresaId");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(150);

                    b.HasKey("Id");

                    b.HasIndex("EmpresaId");

                    b.ToTable("Funcionario");
                });

            modelBuilder.Entity("OnboardingSIGDB1.Domain.Entitys.FuncionarioCargo", b =>
                {
                    b.Property<long>("FuncionarioId");

                    b.Property<long>("CargoId");

                    b.Property<DateTime>("DataVinculacao");

                    b.HasKey("FuncionarioId", "CargoId");

                    b.HasIndex("CargoId");

                    b.ToTable("FuncionarioCargo");
                });

            modelBuilder.Entity("OnboardingSIGDB1.Domain.Entitys.Funcionario", b =>
                {
                    b.HasOne("OnboardingSIGDB1.Domain.Entitys.Empresa", "Empresa")
                        .WithMany("Funcionarios")
                        .HasForeignKey("EmpresaId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("OnboardingSIGDB1.Domain.Entitys.FuncionarioCargo", b =>
                {
                    b.HasOne("OnboardingSIGDB1.Domain.Entitys.Cargo", "Cargo")
                        .WithMany("FuncionarioCargos")
                        .HasForeignKey("CargoId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("OnboardingSIGDB1.Domain.Entitys.Funcionario", "Funcionario")
                        .WithMany("FuncionarioCargos")
                        .HasForeignKey("FuncionarioId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}