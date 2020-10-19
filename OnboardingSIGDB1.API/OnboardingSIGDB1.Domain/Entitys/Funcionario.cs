using OnboardingSIGDB1.Domain.Empresas;
using OnboardingSIGDB1.Domain.Entitys.Validators;
using System;
using System.Collections.Generic;

namespace OnboardingSIGDB1.Domain.Entitys
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

        public string Nome { get; set; }
        public string Cpf { get; set; }
        public DateTime? DataContratacao { get; set; }
        public long? EmpresaId { get; set; }
        public Empresa Empresa { get; set; }
        public IEnumerable<FuncionarioCargo> FuncionarioCargos { get; set; }
    }
}
