using FluentValidation;
using FluentValidation.Results;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnboardingSIGDB1.Domain.Entitys
{
    public abstract class Entity
    {
        public long Id { get; set; }
        [NotMapped]
        public bool Valid { get; private set; }
        [NotMapped]
        public ValidationResult ValidationResult { get; private set; }

        public bool Validate<TModel>(TModel model, AbstractValidator<TModel> validator)
        {
            ValidationResult = validator.Validate(model);
            return Valid = ValidationResult.IsValid;
        }
    }
}
