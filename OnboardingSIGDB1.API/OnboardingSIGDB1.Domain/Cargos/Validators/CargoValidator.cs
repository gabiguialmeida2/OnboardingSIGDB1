using FluentValidation;
using OnboardingSIGDB1.Domain.Utils;

namespace OnboardingSIGDB1.Domain.Cargos.Validators
{
    public class CargoValidator: AbstractValidator<Cargo>
    {
        public CargoValidator()
        {
            RuleFor(e => e.Descricao)
              .NotEmpty()
              .WithMessage(string.Format(MensagensErro.CampoObrigatorio, "Descrição"));

            RuleFor(e => e.Descricao)
            .Length(0, 250)
            .WithMessage(string.Format(MensagensErro.TamanhoString, "Descrição", 1, 250));

        }
    }
}
