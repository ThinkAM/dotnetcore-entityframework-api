using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scheduler.API.ViewModels.Validations
{
    public class CmsViewModelValidator : AbstractValidator<CmsViewModel>
    {
        public CmsViewModelValidator()
        {
            RuleFor(cms => cms.Titulo).NotEmpty().WithMessage("Não Aceita Vazio");
            RuleFor(cms => cms.Avatar).NotEmpty().WithMessage("Escolha um avatar");
            //RuleFor(cms => cms.QtdFields).NotEmpty().WithMessage("Quantidade de Campos");
        }
    }
}
