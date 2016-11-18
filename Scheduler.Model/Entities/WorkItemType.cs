using System.Collections;
using System.Collections.Generic;

namespace Scheduler.Model
{
    public class WorkItemType : IEntityBase           
    {
        public WorkItemType()
        {
            Cmsies = new List<Cms>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Cms> Cmsies { get; set; }
    }
}
