using OnboardingSIGDB1.Domain.Empresas.Validators;
using OnboardingSIGDB1.Domain.Entitys;
using System;
using System.Collections.Generic;

namespace OnboardingSIGDB1.Domain.Empresas
{
    public class Empresa : Entity
    {
        public Empresa()
        {

        }

        public Empresa(string nome, string cnpj, DateTime? dataFundacao) :
            this()
        {
            Nome = nome;
            Cnpj = cnpj;
            DataFundacao = dataFundacao;
            Validate(this, new EmpresaValidator());
        }

        public Empresa(long id, string nome, string cnpj, DateTime? dataFundacao) :
            this(nome, cnpj, dataFundacao)
        {
            Id = id;
        }

        public string Nome { get; private set; }
        public string Cnpj { get; private set; }
        public DateTime? DataFundacao { get; private set; }
        public IEnumerable<Funcionario> Funcionarios { get; private set; }

        public void AlterarNome(string nome)
        {
            Nome = nome;
        }

        public void AlterarCnpj(string cnpj)
        {
            Cnpj = cnpj;
        }

        public void AlterarDataFundacao(DateTime? dataFuncacao)
        {
            DataFundacao = dataFuncacao;
        }

        public void AlterarFuncionarios(IEnumerable<Funcionario> funcionarios)
        {
            Funcionarios = funcionarios;
        }

        public void Validar()
        {
            Validate(this, new EmpresaValidator());
        }
    }
}
