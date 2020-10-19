using Bogus;
using Bogus.DataSets;
using Bogus.Extensions.Brazil;
using OnboardingSIGDB1.Domain.Empresas;
using OnboardingSIGDB1.Domain.Empresas.Dtos;
using OnboardingSIGDB1.Domain.Funcionarios;
using System;
using System.Collections.Generic;

namespace OnboardingSIGDB1.Domain.Tests.EntityBuilders
{
    public class EmpresaBuilder
    {
        private Empresa Empresa { get; set; }
        private IEnumerable<Funcionario> _funcionarios;

        public EmpresaBuilder()
        {
            Empresa = new Faker<Empresa>("pt_BR")
                 .CustomInstantiator(f =>
                     new Empresa(f.Company.CompanyName(), f.Company.Cnpj(includeFormatSymbols: false), f.Date.Past()));
        }

        public EmpresaBuilder WithId(long id)
        {
            Empresa.AlterarId(id);
            return this;
        }

        public EmpresaBuilder WithCnpj(string cnpj)
        {
            Empresa.AlterarCnpj(cnpj);
            return this;
        }

        public EmpresaBuilder WithNome(string nome)
        {
            Empresa.AlterarNome(nome);
            return this;
        }

        public EmpresaBuilder WithFuncionarios(int quantidade)
        {
            _funcionarios = new FuncionarioBuilder(quantidade)
                .BuildList();

            return this;
        }

        public Empresa Build()
        {
            var empresa = new Empresa(Empresa.Id, Empresa.Nome, Empresa.Cnpj, Empresa.DataFundacao);
            empresa.AlterarFuncionarios(_funcionarios);

            return empresa;
        }

        public EmpresaDto BuildDto(Empresa empresa)
        {
            return new EmpresaDto()
            {
                Cnpj = empresa.Cnpj,
                DataFundacao = empresa.DataFundacao,
                Id = empresa.Id,
                Nome = empresa.Nome
            };
        }


    }
}
