using OnboardingSIGDB1.Domain.Cargos.Validators;
using OnboardingSIGDB1.Domain.Funcionarios;
using System.Collections.Generic;

namespace OnboardingSIGDB1.Domain.Cargos
{
    public class Cargo : Entity
    {
        public Cargo(string descricao)
        {
            Descricao = descricao;
            Validate(this, new CargoValidator());
        }

        public Cargo(long id, string descricao) : 
            this(descricao)
        {
            Id = id;
        }

        public string Descricao { get; private set; }
        public IEnumerable<FuncionarioCargo> FuncionarioCargos { get; private set; }

        public void AlterarDescricao(string descricao)
        {
            Descricao = descricao;
        }

        public void AlterarFuncionariosCargos(IEnumerable<FuncionarioCargo> funcionarioCargos)
        {
            FuncionarioCargos = funcionarioCargos;
        }
    }
}
