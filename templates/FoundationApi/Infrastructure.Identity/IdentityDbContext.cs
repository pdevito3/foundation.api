namespace Infrastructure.Identity
{
    using Application.Interfaces;
    using Domain.Common;
    using Infrastructure.Identity.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.ChangeTracking;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

    public class IdentityDbContext : IdentityDbContext<ApplicationUser>
    {
        private readonly IDateTimeService _dateTimeService;

        public IdentityDbContext(
            DbContextOptions<IdentityDbContext> options,
            IDateTimeService dateTime) : base(options)
        {
            _dateTimeService = dateTime;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.HasDefaultSchema("Identity");
            builder.Entity<ApplicationUser>(entity =>
            {
                entity.ToTable(name: "User");
            });

            builder.Entity<IdentityRole>(entity =>
            {
                entity.ToTable(name: "Role");
            });
            builder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.ToTable("UserRoles");
            });
            builder.Entity<IdentityUserClaim<string>>(entity =>
            {
                entity.ToTable("UserClaims");
            });
            builder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.ToTable("UserLogins");
            });
            builder.Entity<IdentityRoleClaim<string>>(entity =>
            {
                entity.ToTable("RoleClaims");

            });
            builder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.ToTable("UserTokens");
            });
        }


        //TODO: Abstract this logic out into an custom inheritable dbcontext
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
