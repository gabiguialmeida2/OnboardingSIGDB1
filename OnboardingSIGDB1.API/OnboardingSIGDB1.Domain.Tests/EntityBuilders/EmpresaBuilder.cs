using Bogus;
using Bogus.Extensions.Brazil;
using OnboardingSIGDB1.Domain.Entitys;

namespace OnboardingSIGDB1.Domain.Tests.EntityBuilders
{
    public class EmpresaBuilder
    {
        private Empresa Empresa { get; set; }

        public EmpresaBuilder()
        {
            Empresa = new Faker<Empresa>("pt_BR")
                .CustomInstantiator(f =>
                    new Empresa()
                    {
                        Cnpj = f.Company.Cnpj(includeFormatSymbols: false),
                        DataFundacao = f.Date.Past(),
                        Nome = f.Company.CompanyName()
                    });
        }

        public EmpresaBuilder WithId(long id)
        {
            Empresa.Id = id;
            return this;
        }

        public EmpresaBuilder WithCnpj(string cnpj)
        {
            Empresa.Cnpj = cnpj;
            return this;
        }

        public EmpresaBuilder WithNome(string nome)
        {
            Empresa.Nome = nome;
            return this;
        }

        public EmpresaBuilder WithFuncionarios(int quantidade)
        {
            Empresa.Funcionarios = new FuncionarioBuilder(quantidade)
                .BuildList();

            return this;
        }

        public Empresa Build()
        {
            return new Empresa(Empresa.Nome, Empresa.Cnpj, Empresa.DataFundacao)
            {
                Id = Empresa.Id,
                Funcionarios = Empresa.Funcionarios
            };
        }


    }
}
