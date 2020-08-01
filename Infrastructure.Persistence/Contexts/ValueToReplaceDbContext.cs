namespace Infrastructure.Persistence.Contexts
{
    using Application.Interfaces;
    using Domain.Common;
    using Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.ChangeTracking;
    using System.Threading;
    using System.Threading.Tasks;

    public class ValueToReplaceDbContext : DbContext
    {
        private readonly IDateTimeService _dateTimeService;

        public ValueToReplaceDbContext(
            DbContextOptions<ValueToReplaceDbContext> options,
            IDateTimeService dateTime) : base(options) 
        {
            _dateTimeService = dateTime;
        }

        public DbSet<ValueToReplace> ValueToReplaces { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            {
                UpdateAuditableFields(entry, _dateTimeService);
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        public override int SaveChanges()
        {
            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            {
                UpdateAuditableFields(entry, _dateTimeService);
            }

            return base.SaveChanges();
        }

        public static void UpdateAuditableFields(EntityEntry<AuditableEntity> entry, IDateTimeService service)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedBy = "TBD User"; //_currentUserService.UserId;
                    entry.Entity.CreatedOn = service.NowUtc;
                    break;
                case EntityState.Modified:
                    entry.Entity.LastModifiedBy = "TBD User"; //_currentUserService.UserId;
                    entry.Entity.LastModifiedOn = service.NowUtc;
                    break;
            }            
        }
    }
}
