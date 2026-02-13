using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class AuthDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // SoftDelete for Identity users
            builder.Entity<ApplicationUser>().HasQueryFilter(u => u.Active);
        }

        public override int SaveChanges()
        {
            ApplyUserDates();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ApplyUserDates();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void ApplyUserDates()
        {
            var now = DateTime.UtcNow;

            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is ApplicationUser user &&
                    (entry.State == EntityState.Added || entry.State == EntityState.Modified))
                {
                    if (entry.State == EntityState.Added)
                        user.CreateDate = now;

                    user.UpdateDate = now;
                }
            }
        }
    }
}
