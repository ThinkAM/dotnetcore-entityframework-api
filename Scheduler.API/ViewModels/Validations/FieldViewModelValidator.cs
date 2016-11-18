using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scheduler.API.ViewModels.Validations
{
    public class FieldViewModelValidator : AbstractValidator<FieldViewModel>
    {
        public FieldViewModelValidator()
        {
            RuleFor(field => field.Name).NotEmpty().WithMessage("Não Aceita Vazio");
            RuleFor(field => field.Type).NotEmpty().WithMessage("Escolha um Tipo");
            RuleFor(field => field.Description).NotEmpty().WithMessage("Escolha uma descrição");
        }
    }
}
