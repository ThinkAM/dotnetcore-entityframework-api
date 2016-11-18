using Scheduler.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scheduler.Data.Abstract
{
    public interface IScheduleRepository : IEntityBaseRepository<Schedule> { }

    public interface IUserRepository : IEntityBaseRepository<User> { }

    public interface IAttendeeRepository : IEntityBaseRepository<Attendee> { }

    public interface IFieldRepository : IEntityBaseRepository<Field> { }

    public interface ICmsRepository : IEntityBaseRepository<Cms> { }

    public interface IWorkItemTypeRepository : IEntityBaseRepository<WorkItemType> { }
}
