using Scheduler.Model;
using Scheduler.Data.Abstract;

namespace Scheduler.Data.Repositories
{
    public class WorkItemTypeRepository : EntityBaseRepository<WorkItemType>, IWorkItemTypeRepository
    {
        public WorkItemTypeRepository(SchedulerContext context)
            : base(context)
        { }
    }
}
