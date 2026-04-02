using System;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options) { }

        public DbSet<MetaPages> MetaPages { get; set; }
        public DbSet<KeyWords> KeyWords { get; set; }
        public DbSet<Pixels> Pixels { get; set; }

        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<InfluencerProfile> InfluencerProfiles { get; set; }
        public DbSet<SocialAccount> SocialAccounts { get; set; }

        public DbSet<Niche> Niches { get; set; }
        public DbSet<UserNiche> UserNiches { get; set; }

        public DbSet<UserKeyWord> UserKeyWords { get; set; }
        public DbSet<VerificationRequest> VerificationRequests { get; set; }

        public DbSet<ServiceListing> ServiceListings { get; set; }
        public DbSet<ServicePricingOption> ServicePricingOptions { get; set; }

        public DbSet<InfluencerMedia> InfluencerMedia { get; set; }
        public DbSet<InfluencerAsset> InfluencerAssets { get; set; }

        public DbSet<BrandProfile> BrandProfiles { get; set; }
        public DbSet<AgencyProfile> AgencyProfiles { get; set; }
        public DbSet<AgencyClient> AgencyClients { get; set; }
        public DbSet<AgencyBrand> AgencyBrands { get; set; }
        public DbSet<InfluencerBusiness> InfluencerBusinesses { get; set; }

        public DbSet<Campaign> Campaigns { get; set; }
        public DbSet<CampaignRequirement> CampaignRequirements { get; set; }
        public DbSet<CampaignInvite> CampaignInvites { get; set; }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDeliverable> OrderDeliverables { get; set; }
        public DbSet<OrderApproval> OrderApprovals { get; set; }
        public DbSet<Dispute> Disputes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>()
                .ToTable("AspNetUsers", t => t.ExcludeFromMigrations());

            modelBuilder.Entity<UserProfile>()
                .HasOne(x => x.User)
                .WithOne(x => x.UserProfile)
                .HasForeignKey<UserProfile>(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<InfluencerProfile>()
                .HasOne(x => x.User)
                .WithOne(x => x.InfluencerProfile)
                .HasForeignKey<InfluencerProfile>(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SocialAccount>()
                .HasOne(x => x.User)
                .WithMany(x => x.SocialAccounts)
                .HasForeignKey(x => x.UserId);

            modelBuilder.Entity<UserNiche>()
                .HasOne(x => x.User)
                .WithMany(x => x.UserNiches)
                .HasForeignKey(x => x.UserId);

            modelBuilder.Entity<UserNiche>()
                .HasOne(x => x.Niche)
                .WithMany(x => x.UserNiches)
                .HasForeignKey(x => x.NicheId);

            modelBuilder.Entity<UserKeyWord>()
                .HasOne(x => x.User)
                .WithMany(x => x.UserKeyWords)
                .HasForeignKey(x => x.UserId);

            modelBuilder.Entity<UserKeyWord>()
                .HasOne(x => x.KeyWords)
                .WithMany(x => x.UserKeyWords)
                .HasForeignKey(x => x.KeyWordsId);

            modelBuilder.Entity<VerificationRequest>()
                .HasOne(x => x.User)
                .WithMany(x => x.VerificationRequests)
                .HasForeignKey(x => x.UserId);

            modelBuilder.Entity<ServiceListing>()
                .HasOne(x => x.Influencer)
                .WithMany(x => x.ServiceListings)
                .HasForeignKey(x => x.InfluencerId);

            modelBuilder.Entity<ServicePricingOption>()
                .HasOne(x => x.ServiceListing)
                .WithMany(x => x.PricingOptions)
                .HasForeignKey(x => x.ServiceListingId);

            modelBuilder.Entity<InfluencerMedia>()
                .HasOne(x => x.Influencer)
                .WithMany(x => x.InfluencerMedia)
                .HasForeignKey(x => x.InfluencerId);

            modelBuilder.Entity<InfluencerAsset>()
                .HasOne(x => x.Influencer)
                .WithMany(x => x.InfluencerAssets)
                .HasForeignKey(x => x.InfluencerId);

            modelBuilder.Entity<BrandProfile>()
                .HasOne(x => x.User)
                .WithOne(x => x.BrandProfile)
                .HasForeignKey<BrandProfile>(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AgencyProfile>()
                .HasOne(x => x.User)
                .WithOne(x => x.AgencyProfile)
                .HasForeignKey<AgencyProfile>(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AgencyClient>()
                .HasOne(x => x.AgencyProfile)
                .WithMany(x => x.AgencyClients)
                .HasForeignKey(x => x.AgencyProfileId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AgencyClient>()
                .HasOne(x => x.BrandProfile)
                .WithMany(x => x.AgencyClients)
                .HasForeignKey(x => x.BrandProfileId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AgencyClient>()
                .HasIndex(x => new { x.AgencyProfileId, x.BrandProfileId })
                .IsUnique()
                .HasFilter("[Active] = 1");

            modelBuilder.Entity<AgencyBrand>()
                .HasOne(x => x.Agency)
                .WithMany(x => x.AgencyBrands)
                .HasForeignKey(x => x.AgencyId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AgencyBrand>()
                .HasOne(x => x.Brand)
                .WithMany(x => x.AgencyBrands)
                .HasForeignKey(x => x.BrandId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AgencyBrand>()
                .HasIndex(x => new { x.AgencyId, x.BrandId })
                .IsUnique()
                .HasFilter("[Active] = 1");

            modelBuilder.Entity<InfluencerBusiness>()
                .HasOne(x => x.Influencer)
                .WithMany(x => x.InfluencerBusinesses)
                .HasForeignKey(x => x.InfluencerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SocialAccount>()
                .Property(x => x.EngagementRate)
                .HasPrecision(10, 2);

            modelBuilder.Entity<ServiceListing>()
                .Property(x => x.BasePrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<ServicePricingOption>()
                .Property(x => x.Price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<InfluencerAsset>()
                .Property(x => x.RetailPrice)
                .HasPrecision(18, 2);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (entityType.ClrType.IsSubclassOf(typeof(Common)))
                {
                    var method = typeof(AppDBContext)
                        .GetMethod(nameof(SetSoftDeleteFilter),
                            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                        ?.MakeGenericMethod(entityType.ClrType);

                    method?.Invoke(null, new object[] { modelBuilder });
                }
            }

            modelBuilder.Entity<Campaign>()
                .HasOne(x => x.BrandProfile)
                .WithMany(x => x.Campaigns)
                .HasForeignKey(x => x.BrandProfileId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Campaign>()
                .HasOne(x => x.AgencyProfile)
                .WithMany(x => x.Campaigns)
                .HasForeignKey(x => x.AgencyProfileId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CampaignRequirement>()
                .HasOne(x => x.Campaign)
                .WithOne(x => x.CampaignRequirement)
                .HasForeignKey<CampaignRequirement>(x => x.CampaignId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CampaignInvite>()
                .HasOne(x => x.Campaign)
                .WithMany(x => x.CampaignInvites)
                .HasForeignKey(x => x.CampaignId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CampaignInvite>()
                .HasOne(x => x.Influencer)
                .WithMany(x => x.CampaignInvites)
                .HasForeignKey(x => x.InfluencerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CampaignInvite>()
                .HasIndex(x => new { x.CampaignId, x.InfluencerId })
                .IsUnique()
                .HasFilter("[Active] = 1");

            modelBuilder.Entity<Campaign>()
                .Property(x => x.TotalBudget)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Order>()
                .HasOne(x => x.Campaign)
                .WithMany()
                .HasForeignKey(x => x.CampaignId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Order>()
                .HasOne(x => x.Buyer)
                .WithMany(x => x.BuyerOrders)
                .HasForeignKey(x => x.BuyerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Order>()
                .HasOne(x => x.Influencer)
                .WithMany(x => x.InfluencerOrders)
                .HasForeignKey(x => x.InfluencerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Order>()
                .Property(x => x.AgreedPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<OrderDeliverable>()
                .HasOne(x => x.Order)
                .WithMany()
                .HasForeignKey(x => x.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderApproval>()
                .HasOne(x => x.Order)
                .WithMany()
                .HasForeignKey(x => x.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Dispute>()
                .HasOne(x => x.Order)
                .WithMany()
                .HasForeignKey(x => x.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        private static void SetSoftDeleteFilter<TEntity>(ModelBuilder modelBuilder)
            where TEntity : Common
        {
            modelBuilder.Entity<TEntity>().HasQueryFilter(x => x.Active);
        }

        public override int SaveChanges()
        {
            ApplyAudit();
            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            ApplyAudit();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ApplyAudit();
            return base.SaveChangesAsync(cancellationToken);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            ApplyAudit();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void ApplyAudit()
        {
            var now = DateTime.UtcNow;

            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is Common e)
                {
                    if (entry.State == EntityState.Added)
                    {
                        e.CreateDate = now;
                        e.UpdateDate = now;
                    }
                    else if (entry.State == EntityState.Modified)
                    {
                        e.UpdateDate = now;
                    }
                }

                if (entry.Entity is ApplicationUser u)
                {
                    if (entry.State == EntityState.Added)
                    {
                        u.CreateDate = now;
                        u.UpdateDate = now;
                    }
                    else if (entry.State == EntityState.Modified)
                    {
                        u.UpdateDate = now;
                    }
                }
            }
        }
    }
}