using System.Collections.Generic;

namespace Scheduler.Model
{
    //Precisa do IEntityBase pra poder criar o IRepository
    public class Cms : IEntityBase
    {
        public Cms()
        {
            Fields = new List<Field>();
        }

        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Avatar { get; set; }
        public int WorkItemTypeId { get; set; }
        public virtual WorkItemType WorkItemType { get; set; }
        public int QtdFields { get; set; }
        public int? FieldsCreated { get; set; }
        public virtual ICollection<Field> Fields { get; set; }
    }
}
