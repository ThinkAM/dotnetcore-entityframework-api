﻿using Scheduler.Model;
using Scheduler.Data.Abstract;

namespace Scheduler.Data.Repositories
{
    public class AttendeeRepository : EntityBaseRepository<Attendee>, IAttendeeRepository
    {
        public AttendeeRepository(SchedulerContext context)
            : base(context)
        { }
    }
}
