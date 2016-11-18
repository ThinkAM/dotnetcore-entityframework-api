using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Scheduler.Model;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Scheduler.Data
{
    public class SchedulerContext : DbContext
    {
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Attendee> Attendees { get; set; }
        public DbSet<Cms> Cmsies { get; set; }
        public DbSet<Field> Fields { get; set; }
        public DbSet<WorkItemType> WorkItemTypes { get; set; }

        public SchedulerContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            modelBuilder.Entity<Schedule>()
                .ToTable("Schedule");

            modelBuilder.Entity<Schedule>()
                .Property(s => s.CreatorId)
                .IsRequired();

            modelBuilder.Entity<Schedule>()
                .Property(s => s.DateCreated)
                .HasDefaultValue(DateTime.Now);

            modelBuilder.Entity<Schedule>()
                .Property(s => s.DateUpdated)
                .HasDefaultValue(DateTime.Now);

            modelBuilder.Entity<Schedule>()
                .Property(s => s.Type)
                .HasDefaultValue(ScheduleType.Work);

            modelBuilder.Entity<Schedule>()
                .Property(s => s.Status)
                .HasDefaultValue(ScheduleStatus.Valid);

            modelBuilder.Entity<Schedule>()
                .HasOne(s => s.Creator)
                .WithMany(c => c.SchedulesCreated);

            modelBuilder.Entity<User>()
              .ToTable("User");

            modelBuilder.Entity<User>()
                .Property(u => u.Name)
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder.Entity<Attendee>()
              .ToTable("Attendee");

            modelBuilder.Entity<Attendee>()
                .HasOne(a => a.User)
                .WithMany(u => u.SchedulesAttended)
                .HasForeignKey(a => a.UserId);

            modelBuilder.Entity<Attendee>()
                .HasOne(a => a.Schedule)
                .WithMany(s => s.Attendees)
                .HasForeignKey(a => a.ScheduleId);

            modelBuilder.Entity<Cms>()
                .ToTable("Cms");

            modelBuilder.Entity<Cms>()
                .HasOne(prop => prop.WorkItemType)
                .WithMany(w => w.Cmsies)
                .HasForeignKey(prop => prop.WorkItemTypeId);

            modelBuilder.Entity<Cms>()
                .Property(prop => prop.Titulo)
                .HasMaxLength(50)
                .IsRequired();

            modelBuilder.Entity<Cms>()
                .Property(prop => prop.WorkItemTypeId)
                .IsRequired();

            modelBuilder.Entity<Field>()
                .ToTable("Field");

            modelBuilder.Entity<Field>()
                .HasOne(a => a.Cms)
                .WithMany(u => u.Fields)
                .HasForeignKey(u => u.CmsId);

            modelBuilder.Entity<Field>()
                .Property(prop => prop.Name)
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder.Entity<Field>()
                .Property(prop => prop.Type)
                .HasMaxLength(50)
                .IsRequired();

            modelBuilder.Entity<Field>()
                .Property(prop => prop.Description)
                .HasMaxLength(200)
                .IsRequired();

            modelBuilder.Entity<Field>()
                 .Property(prop => prop.CmsId)
                 .IsRequired();

            modelBuilder.Entity<Field>()
                .Property(prop => prop.DateCreated)
                .HasDefaultValue(DateTime.UtcNow);

            modelBuilder.Entity<Field>()
                .Property(prop => prop.DateUpdated)
                .HasDefaultValue(DateTime.UtcNow);

            modelBuilder.Entity<WorkItemType>()
                .ToTable("WorkItemType");

            modelBuilder.Entity<WorkItemType>()
               .Property(prop => prop.Name)
               .HasMaxLength(100)
               .IsRequired();
        }
    }
}
