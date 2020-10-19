using OnboardingSIGDB1.Domain.Empresas;
using OnboardingSIGDB1.Domain.Funcionarios.Validators;
using System;
using System.Collections.Generic;

namespace OnboardingSIGDB1.Domain.Funcionarios
{
    public class Funcionario: Entity
    {
        public Funcionario()
        {

        }

        public Funcionario(string nome, string cpf, DateTime? dataContratacao) :
            this()
        {
            Nome = nome;
            Cpf = cpf;
            DataContratacao = dataContratacao;
            Validate(this, new FuncionarioValidator());
        }

        public Funcionario(long id, string nome, string cpf, DateTime? dataContratacao) :
           this(nome, cpf, dataContratacao)
        {
            Id = id;
        }

        public string Nome { get; private set; }
        public string Cpf { get; private set; }
        public DateTime? DataContratacao { get; private set; }
        public long? EmpresaId { get; private set; }
        public Empresa Empresa { get; set; }
        public IEnumerable<FuncionarioCargo> FuncionarioCargos { get; private set; }

        public void AlterarNome(string nome)
        {
            Nome = nome;
        }

        public void AlterarCpf(string cpf)
        {
            Cpf = cpf;
        }

        public void AlterarDataContratacao(DateTime? dataContratacao)
        {
            DataContratacao = dataContratacao;
        }

        public void AlterarEmpresaId(long? empresaId)
        {
            EmpresaId = empresaId;
        }

        public void AlterarFuncionarioCargos(IEnumerable<FuncionarioCargo> funcionarioCargos)
        {
            FuncionarioCargos = funcionarioCargos;
        }
    }
}
