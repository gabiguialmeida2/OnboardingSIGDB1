using OnboardingSIGDB1.Domain.Entitys.Validators;
using System.Collections.Generic;

namespace OnboardingSIGDB1.Domain.Entitys
{
    public class Cargo:Entity
    {
        public Cargo(string descricao)
        {
            Descricao = descricao;
            Validate(this, new CargoValidator());
        }

        public string Descricao { get; set; }
        public IEnumerable<FuncionarioCargo> FuncionarioCargos { get; set; }
    }
}
