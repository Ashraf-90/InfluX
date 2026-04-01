using Domain.Entities;

namespace Domain.Abstractions
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<MetaPages> MetaPages { get; }
        IRepository<KeyWords> KeyWords { get; }
        IRepository<Pixels> Pixels { get; }

        IRepository<UserProfile> UserProfiles { get; }
        IRepository<InfluencerProfile> InfluencerProfiles { get; }
        IRepository<SocialAccount> SocialAccounts { get; }

        IRepository<Niche> Niches { get; }
        IRepository<UserNiche> UserNiches { get; }

        IRepository<UserKeyWord> UserKeyWords { get; }
        IRepository<VerificationRequest> VerificationRequests { get; }

        IRepository<ServiceListing> ServiceListings { get; }
        IRepository<ServicePricingOption> ServicePricingOptions { get; }

        IRepository<InfluencerMedia> InfluencerMedia { get; }
        IRepository<InfluencerAsset> InfluencerAssets { get; }

        IRepository<BrandProfile> BrandProfiles { get; }
        IRepository<AgencyProfile> AgencyProfiles { get; }
        IRepository<AgencyClient> AgencyClients { get; }

        // NEW
        IRepository<AgencyBrand> AgencyBrands { get; }

        IRepository<InfluencerBusiness> InfluencerBusinesses { get; }
        IRepository<Campaign> Campaigns { get; }
        IRepository<CampaignRequirement> CampaignRequirements { get; }
        IRepository<CampaignInvite> CampaignInvites { get; }
        IRepository<Order> Orders { get; }
        IRepository<OrderDeliverable> OrderDeliverables { get; }
        IRepository<OrderApproval> OrderApprovals { get; }
        IRepository<Dispute> Disputes { get; }

        Task<int> CompleteAsync();
        Task<int> SaveChangesAsync();
    }
}