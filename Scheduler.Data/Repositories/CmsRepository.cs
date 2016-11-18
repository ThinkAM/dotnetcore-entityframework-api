using Scheduler.Model;
using Scheduler.Data.Abstract;

namespace Scheduler.Data.Repositories
{
    public class CmsRepository : EntityBaseRepository<Cms>, ICmsRepository
    {
        public CmsRepository(SchedulerContext context)
            : base(context)
        { }
    }
}
