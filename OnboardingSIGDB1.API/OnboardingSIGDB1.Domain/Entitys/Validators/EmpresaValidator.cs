using FluentValidation;
using OnboardingSIGDB1.Domain.Utils;
using System;

namespace OnboardingSIGDB1.Domain.Entitys.Validators
{
    public class EmpresaValidator : AbstractValidator<Empresa>
    {
        public EmpresaValidator()
        {
            RuleFor(e => e.Nome)
                .NotEmpty()
                .WithMessage(string.Format(MensagensErro.CampoObrigatorio, "Nome"));

            RuleFor(e => e.Nome)
              .Length(0, 150)
              .WithMessage(string.Format(MensagensErro.TamanhoString, "Nome", 1, 150));

            RuleFor(e => e.Cnpj)
                .NotEmpty()
                .Length(14)
                .WithMessage(string.Format($"{MensagensErro.CampoObrigatorio} e deve conter 14 caracteres", "Cnpj"));

            RuleFor(e => e.DataFundacao)
                .GreaterThan(DateTime.MinValue)
                .WithMessage($"A data de fundação deve ser maior que {DateTime.MinValue.ToShortDateString()}");
        }
    }
}
