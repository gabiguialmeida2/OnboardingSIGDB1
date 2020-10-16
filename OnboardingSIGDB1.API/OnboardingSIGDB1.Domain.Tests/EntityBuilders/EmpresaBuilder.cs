using Bogus;
using Bogus.DataSets;
using Bogus.Extensions.Brazil;
using OnboardingSIGDB1.Domain.Empresas;
using OnboardingSIGDB1.Domain.Empresas.Dtos;
using OnboardingSIGDB1.Domain.Entitys;
using System;
using System.Collections.Generic;

namespace OnboardingSIGDB1.Domain.Tests.EntityBuilders
{
    public class EmpresaBuilder
    {
        private long _id;
        private string _cnpj;
        private string _nome;
        private DateTime _data;
        private IEnumerable<Funcionario> _funcionarios;

        public EmpresaBuilder()
        {
            var company = new Faker<Company>().Generate();
            _cnpj = company.Cnpj(includeFormatSymbols: false);
            _nome = company.CompanyName();
            _data = new Faker("pt_BR").Date.Past();
        }

        public EmpresaBuilder WithId(long id)
        {
            _id = id;
            return this;
        }

        public EmpresaBuilder WithCnpj(string cnpj)
        {
            _cnpj = cnpj;
            return this;
        }

        public EmpresaBuilder WithNome(string nome)
        {
            _nome = nome;
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
            var empresa = new Empresa(_id, _nome, _cnpj, _data);
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
