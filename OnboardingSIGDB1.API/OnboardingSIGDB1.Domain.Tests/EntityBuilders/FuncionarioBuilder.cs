﻿using Bogus;
using Bogus.Extensions.Brazil;
using OnboardingSIGDB1.Domain.Entitys;
using System.Collections.Generic;

namespace OnboardingSIGDB1.Domain.Tests.EntityBuilders
{
    public class FuncionarioBuilder
    {
        private Funcionario Funcionario { get; set; }
        private List<Funcionario> Funcionarios { get; set; }

        public FuncionarioBuilder(int quantidade)
        {
            var funcionarios = new Faker<Funcionario>("pt_BR")
                   .CustomInstantiator(f =>
                       new Funcionario()
                       {
                           Cpf = f.Person.Cpf(includeFormatSymbols: false),
                           DataContratacao = f.Date.Past(),
                           Nome = f.Person.FullName,
                           Id = f.Random.Long(min: 0)
                       });

            Funcionarios = funcionarios.Generate(quantidade);
        }

        public FuncionarioBuilder()
        {
            Funcionario = new Faker<Funcionario>("pt_BR")
                   .CustomInstantiator(f =>
                       new Funcionario()
                       {
                           Cpf = f.Person.Cpf(includeFormatSymbols: false),
                           DataContratacao = f.Date.Past(),
                           Nome = f.Person.FullName
                       });
        }

        public FuncionarioBuilder WithId(long id)
        {
            Funcionario.Id = id;
            return this;
        }

        public FuncionarioBuilder WithCpf(string cpf)
        {
            Funcionario.Cpf = cpf;
            return this;
        }

        public FuncionarioBuilder WithNome(string nome)
        {
            Funcionario.Nome = nome;
            return this;
        }

        public FuncionarioBuilder WithCargos(int quantidade)
        {
            List<FuncionarioCargo> funcionarioCargos = new List<FuncionarioCargo>();
            var cargos = new CargoBuilder(quantidade).BuildList();
            for (int i = 0; i < quantidade; i++)
            {
                var cargo = cargos[i];

                funcionarioCargos.Add(
                    new FuncionarioCargo(
                        Funcionario.Id,
                        cargo.Id,
                        new Faker().Date.Past())
                        {
                            Cargo = cargo,
                            Funcionario = Funcionario
                        }
                    );
            }

            Funcionario.FuncionarioCargos = funcionarioCargos;
            return this;
        }

        public FuncionarioBuilder WithEmpresa()
        {
            Funcionario.Empresa = new EmpresaBuilder()
                .WithId(1)
                .Build();

            Funcionario.EmpresaId = Funcionario.Empresa.Id;

            return this;
        }

        public Funcionario Build()
        {
            return new Funcionario(Funcionario.Nome,
                Funcionario.Cpf,
                Funcionario.DataContratacao)
            {
                Id = Funcionario.Id,
                Empresa = Funcionario.Empresa,
                EmpresaId = Funcionario.EmpresaId,
                FuncionarioCargos = Funcionario.FuncionarioCargos
            };
        }

        public List<Funcionario> BuildList()
        {
            return Funcionarios;
        }
    }
}
