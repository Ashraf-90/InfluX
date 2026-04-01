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

        // =========================
        // Domain Tables
        // =========================
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

        // =========================
        // NEW Tables (Outside green box)
        // =========================
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

            // =========================
            // IMPORTANT:
            // Use Identity users table, but DO NOT create it from AppDBContext migrations
            // =========================
            modelBuilder.Entity<ApplicationUser>()
                .ToTable("AspNetUsers", t => t.ExcludeFromMigrations());

            // =========================
            // Relationships to ApplicationUser (AspNetUsers)
            // =========================

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

            // =========================
            // NEW: BrandProfile (1:1)
            // =========================
            modelBuilder.Entity<BrandProfile>()
                .HasOne(x => x.User)
                .WithOne(x => x.BrandProfile)
                .HasForeignKey<BrandProfile>(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // =========================
            // NEW: AgencyProfile (1:1)
            // =========================
            modelBuilder.Entity<AgencyProfile>()
                .HasOne(x => x.User)
                .WithOne(x => x.AgencyProfile)
                .HasForeignKey<AgencyProfile>(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // =========================
            // NEW: AgencyClients (AgencyId + BrandId => Users)
            // =========================
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

            // منع تكرار نفس العلاقة AgencyProfile + BrandProfile (مع SoftDelete)
            modelBuilder.Entity<AgencyClient>()
                .HasIndex(x => new { x.AgencyProfileId, x.BrandProfileId })
                .IsUnique()
                .HasFilter("[Active] = 1"); // SQL Server


            // =========================
            // NEW: AgencyBrands (AgencyProfile + BrandProfile)
            // =========================
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

            // منع تكرار نفس العلاقة AgencyProfile + BrandProfile (مع SoftDelete)
            modelBuilder.Entity<AgencyBrand>()
                .HasIndex(x => new { x.AgencyId, x.BrandId })
                .IsUnique()
                .HasFilter("[Active] = 1");

            // =========================
            // NEW: InfluencerBusiness (1:N)
            // =========================
            modelBuilder.Entity<InfluencerBusiness>()
                .HasOne(x => x.Influencer)
                .WithMany(x => x.InfluencerBusinesses)
                .HasForeignKey(x => x.InfluencerId)
                .OnDelete(DeleteBehavior.Cascade);

            // =========================
            // Decimal precision (remove truncation warnings)
            // =========================
            modelBuilder.Entity<SocialAccount>().Property(x => x.EngagementRate).HasPrecision(10, 2);
            modelBuilder.Entity<ServiceListing>().Property(x => x.BasePrice).HasPrecision(18, 2);
            modelBuilder.Entity<ServicePricingOption>().Property(x => x.Price).HasPrecision(18, 2);
            modelBuilder.Entity<InfluencerAsset>().Property(x => x.RetailPrice).HasPrecision(18, 2);

            // =========================
            // SoftDelete filter for Common entities
            // =========================
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


            // =========================
            // NEW: Campaigns
            // =========================
            modelBuilder.Entity<Campaign>()
                .HasOne(x => x.Brand)
                .WithMany(x => x.BrandCampaigns)
                .HasForeignKey(x => x.BrandId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Campaign>()
                .HasOne(x => x.Agency)
                .WithMany(x => x.AgencyCampaigns)
                .HasForeignKey(x => x.AgencyId)
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

            modelBuilder.Entity<Campaign>().Property(x => x.TotalBudget).HasPrecision(18, 2);


            // =========================
            // NEW: Orders
            // =========================
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
                .HasOne(x => x.ServiceListing)
                .WithMany(x => x.Orders)
                .HasForeignKey(x => x.ServiceListingId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OrderDeliverable>()
                .HasOne(x => x.Order)
                .WithMany(x => x.OrderDeliverables)
                .HasForeignKey(x => x.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderApproval>()
                .HasOne(x => x.Order)
                .WithMany(x => x.OrderApprovals)
                .HasForeignKey(x => x.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderApproval>()
                .HasOne(x => x.ApprovedByUser)
                .WithMany(x => x.OrderApprovals)
                .HasForeignKey(x => x.ApprovedBy)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Dispute>()
                .HasOne(x => x.Order)
                .WithMany(x => x.Disputes)
                .HasForeignKey(x => x.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Dispute>()
                .HasOne(x => x.OpenedByUser)
                .WithMany(x => x.OpenedDisputes)
                .HasForeignKey(x => x.OpenedBy)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Order>().Property(x => x.AgreedPrice).HasPrecision(18, 2);
        }

        private static void SetSoftDeleteFilter<TEntity>(ModelBuilder modelBuilder) where TEntity : Common
        {
            modelBuilder.Entity<TEntity>().HasQueryFilter(x => x.Active);
        }

        public override int SaveChanges()
        {
            ApplyCommonDates();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ApplyCommonDates();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void ApplyCommonDates()
        {
            var now = DateTime.UtcNow;

            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is Common common &&
                    (entry.State == EntityState.Added || entry.State == EntityState.Modified))
                {
                    if (entry.State == EntityState.Added)
                        common.CreateDate = now;

                    common.UpdateDate = now;
                }
            }
        }
    }
}