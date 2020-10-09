using Bogus;
using OnboardingSIGDB1.Domain.Entitys;
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
                    new Cargo(string.Concat(f.Lorem.Letter(10)))
                    {
                        Id = f.Random.Long(min: 0)
                    });

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
            Cargo.Descricao = descricao;
            return this;
        }

        public CargoBuilder WithId(long id)
        {
            Cargo.Id = id;
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
                   }
                   );
            }

            Cargo.FuncionarioCargos = funcionarioCargos;

            return this;
        }

        public Cargo Build()
        {
            return new Cargo(Cargo.Descricao)
            {
                Id = Cargo.Id,
                FuncionarioCargos = Cargo.FuncionarioCargos
            };
        }

        public List<Cargo> BuildList()
        {
            return Cargos; ;
        }

    }

}
