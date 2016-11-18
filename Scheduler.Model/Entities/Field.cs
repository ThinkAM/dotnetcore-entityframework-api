using System;

namespace Scheduler.Model
{
    public class Field : IEntityBase
    {
        public int Id { get; set; }

        public int CmsId { get; set; }
        public virtual Cms Cms { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
    }
}
