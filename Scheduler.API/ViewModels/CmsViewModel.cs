using Scheduler.API.ViewModels.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Scheduler.API.ViewModels
{
    public class CmsViewModel : IValidatableObject
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Avatar { get; set; }
        public int QtdFields { get; set; }
        public int FieldsCreated { get; set; }        
        public int WorkItemTypeId { get; set; }
        public string WorkItemTypeName { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
            {
                var validator = new CmsViewModelValidator();
                var result = validator.Validate(this);
                return result.Errors.Select(item => new ValidationResult(item.ErrorMessage, new[] { item.PropertyName }));
            }
        
    }
}
