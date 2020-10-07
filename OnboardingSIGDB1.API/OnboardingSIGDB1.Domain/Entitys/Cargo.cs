using OnboardingSIGDB1.Domain.Entitys.Validators;

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
    }
}
