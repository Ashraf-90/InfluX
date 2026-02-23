using System;
using System.Threading;
using System.Threading.Tasks;
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

            // =========================
            // IMPORTANT: Ignore Domain entities in AuthDbContext
            // =========================
            builder.Ignore<UserProfile>();
            builder.Ignore<InfluencerProfile>();
            builder.Ignore<SocialAccount>();
            builder.Ignore<Niche>();
            builder.Ignore<UserNiche>();
            builder.Ignore<UserKeyWord>();
            builder.Ignore<VerificationRequest>();
            builder.Ignore<ServiceListing>();
            builder.Ignore<ServicePricingOption>();
            builder.Ignore<InfluencerMedia>();
            builder.Ignore<InfluencerAsset>();

            builder.Ignore<KeyWords>();
            builder.Ignore<MetaPages>();
            builder.Ignore<Pixels>();

            // =========================
            // NEW ENTITIES (must be ignored here)
            // =========================
            builder.Ignore<BrandProfile>();
            builder.Ignore<AgencyProfile>();
            builder.Ignore<AgencyClient>();
            builder.Ignore<InfluencerBusiness>();

            // =========================
            // Also ignore new navigations on ApplicationUser
            // (so AuthDbContext doesn't try to map relationships)
            // =========================
            builder.Entity<ApplicationUser>().Ignore(x => x.BrandProfile);
            builder.Entity<ApplicationUser>().Ignore(x => x.AgencyProfile);
            builder.Entity<ApplicationUser>().Ignore(x => x.AgencyClientsAsAgency);
            builder.Entity<ApplicationUser>().Ignore(x => x.AgencyClientsAsBrand);
            builder.Entity<ApplicationUser>().Ignore(x => x.InfluencerBusinesses);

            // =========================
            // Seed Default Roles
            // =========================
            var influencerRoleId = new Guid("11111111-1111-1111-1111-111111111111");
            var brandRoleId = new Guid("22222222-2222-2222-2222-222222222222");
            var agencyRoleId = new Guid("33333333-3333-3333-3333-333333333333");
            var userRoleId = new Guid("44444444-4444-4444-4444-444444444444");

            builder.Entity<IdentityRole<Guid>>().HasData(
                new IdentityRole<Guid>
                {
                    Id = influencerRoleId,
                    Name = "Influencer",
                    NormalizedName = "INFLUENCER",
                    ConcurrencyStamp = "ROLE-INFLUENCER"
                },
                new IdentityRole<Guid>
                {
                    Id = brandRoleId,
                    Name = "Brand",
                    NormalizedName = "BRAND",
                    ConcurrencyStamp = "ROLE-BRAND"
                },
                new IdentityRole<Guid>
                {
                    Id = agencyRoleId,
                    Name = "Agency",
                    NormalizedName = "AGENCY",
                    ConcurrencyStamp = "ROLE-AGENCY"
                },
                new IdentityRole<Guid>
                {
                    Id = userRoleId,
                    Name = "User",
                    NormalizedName = "USER",
                    ConcurrencyStamp = "ROLE-USER"
                }
            );
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