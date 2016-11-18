using Scheduler.Model;
using Scheduler.Data.Abstract;

namespace Scheduler.Data.Repositories
{
    public class FieldRepository : EntityBaseRepository<Field>, IFieldRepository
    {
        public FieldRepository(SchedulerContext context)
            : base(context)
        { }
    }
}
