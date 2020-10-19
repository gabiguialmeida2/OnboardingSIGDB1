using Bogus;
using OnboardingSIGDB1.Domain.Cargos;
using OnboardingSIGDB1.Domain.Cargos.Dtos;
using OnboardingSIGDB1.Domain.Funcionarios;
using System.Collections.Generic;

namespace OnboardingSIGDB1.Domain.Tests.EntityBuilders
{
    public class CargoBuilder
    {
        private Cargo Cargo { get; set; }
        private List<Cargo> Cargos { get; set; }

        public CargoBuilder(int quantidade)
        {
            var cargos = new Faker<Cargo>("pt_BR")
                .CustomInstantiator(f =>
                    new Cargo(f.Random.Long(min: 1),string.Concat(f.Lorem.Letter(10))));

            Cargos = cargos.Generate(quantidade);
        }

        public CargoBuilder()
        {
            Cargo = new Faker<Cargo>("pt_BR")
                .CustomInstantiator(f =>
                    new Cargo(string.Concat(f.Lorem.Letter(10))));
        }

        public CargoBuilder WithDescricao(string descricao)
        {
            Cargo.AlterarDescricao(descricao);
            return this;
        }

        public CargoBuilder WithId(long id)
        {
            Cargo.AlterarId(id);
            return this;
        }

        public CargoBuilder WithFuncionarios(int quantidade)
        {
            var funcionarioCargos = new List<FuncionarioCargo>();

            var funcionarios = new FuncionarioBuilder(quantidade)
                .BuildList();

            for (int i = 0; i < quantidade; i++)
            {
                var funcionario = funcionarios[i];
                funcionarioCargos.Add(
                   new FuncionarioCargo(
                       funcionario.Id,
                       Cargo.Id,
                       new Faker().Date.Past())
                   {
                       Cargo = Cargo,
                       Funcionario = funcionario
                   });
            }

            Cargo.AlterarFuncionariosCargos(funcionarioCargos);

            return this;
        }

        public Cargo Build()
        {
            var cargo = new Cargo(Cargo.Descricao);
            cargo.AlterarId(Cargo.Id);
            cargo.AlterarFuncionariosCargos(Cargo.FuncionarioCargos);
            return cargo;
        }

        public CargoDto BuildDto()
        {
            return new CargoDto { Descricao = Cargo.Descricao, Id = Cargo.Id };
        }

        public List<Cargo> BuildList()
        {
            return Cargos;
        }

    }

}
