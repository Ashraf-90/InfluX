using Domain.Entities;
using Domain.Entities.Domain.Entities;

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

        Task<int> CompleteAsync();
        Task<int> SaveChangesAsync();
    }
}
