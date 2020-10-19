using Bogus;
using Bogus.Extensions.Brazil;
using OnboardingSIGDB1.Domain.Funcionarios;
using OnboardingSIGDB1.Domain.Funcionarios.Dtos;
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
                       new Funcionario(f.Random.Long(min: 1), f.Person.FullName, f.Person.Cpf(includeFormatSymbols: false), f.Date.Past()));

            Funcionarios = funcionarios.Generate(quantidade);
        }

        public FuncionarioBuilder()
        {
            Funcionario = new Faker<Funcionario>("pt_BR")
                   .CustomInstantiator(f =>
                       new Funcionario(f.Person.FullName, f.Person.Cpf(includeFormatSymbols: false), f.Date.Past()));
        }

        public FuncionarioBuilder WithId(long id)
        {
            Funcionario.AlterarId(id);
            return this;
        }

        public FuncionarioBuilder WithCpf(string cpf)
        {
            Funcionario.AlterarCpf(cpf);
            return this;
        }

        public FuncionarioBuilder WithNome(string nome)
        {
            Funcionario.AlterarNome(nome);
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

            Funcionario.AlterarFuncionarioCargos(funcionarioCargos);
            return this;
        }

        public FuncionarioBuilder WithEmpresa()
        {
            Funcionario.Empresa = new EmpresaBuilder()
                .WithId(1)
                .Build();

            Funcionario.AlterarEmpresaId(Funcionario.Empresa.Id);

            return this;
        }

        public Funcionario Build()
        {
            var funcionario = new Funcionario(Funcionario.Id,
                Funcionario.Nome,
                Funcionario.Cpf,
                Funcionario.DataContratacao);
            funcionario.AlterarFuncionarioCargos(Funcionario.FuncionarioCargos);
            funcionario.AlterarEmpresaId(Funcionario.EmpresaId);
            return funcionario;
        }

        public FuncionarioDto BuildDto()
        {
            return new FuncionarioDto
            {
                Id = Funcionario.Id,
                Nome = Funcionario.Nome,
                Cpf = Funcionario.Cpf,
                DataContratacao = Funcionario.DataContratacao
            };
        }

        public List<Funcionario> BuildList()
        {
            return Funcionarios;
        }
    }
}
