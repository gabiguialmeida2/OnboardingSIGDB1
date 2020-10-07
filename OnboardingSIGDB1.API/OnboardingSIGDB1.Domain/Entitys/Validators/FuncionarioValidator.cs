using FluentValidation;
using OnboardingSIGDB1.Domain.Utils;
using System;

namespace OnboardingSIGDB1.Domain.Entitys.Validators
{
    public class FuncionarioValidator : AbstractValidator<Funcionario>
    {
        public FuncionarioValidator()
        {
            RuleFor(e => e.Nome)
                .NotEmpty()
                .WithMessage(string.Format(MensagensErro.CampoObrigatorio, "Nome"));

            RuleFor(e => e.Nome)
              .Length(0, 150)
              .WithMessage(string.Format(MensagensErro.TamanhoString, "Nome", 1, 150));

            RuleFor(e => e.Cpf)
                .NotEmpty()
                .Length(11)
                .WithMessage(string.Format($"{MensagensErro.CampoObrigatorio} e deve conter 11 caracteres", "Cnpj"));

            RuleFor(e => e.DataContratacao)
                .GreaterThan(DateTime.MinValue)
                .WithMessage($"A data de contratação deve ser maior que {DateTime.MinValue.ToShortDateString()}");
        }
    }
}
