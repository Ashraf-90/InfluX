using Domain.Abstractions;
using Domain.Entities;

namespace Infrastructure.Persistence.Reposities
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDBContext _context;

        private IRepository<MetaPages> _MetaPages;
        private IRepository<KeyWords> _KeyWords;
        private IRepository<Pixels> _Pixels;

        private IRepository<UserProfile> _UserProfiles;
        private IRepository<InfluencerProfile> _InfluencerProfiles;
        private IRepository<SocialAccount> _SocialAccounts;

        private IRepository<Niche> _Niches;
        private IRepository<UserNiche> _UserNiches;

        private IRepository<UserKeyWord> _UserKeyWords;
        private IRepository<VerificationRequest> _VerificationRequests;

        private IRepository<ServiceListing> _ServiceListings;
        private IRepository<ServicePricingOption> _ServicePricingOptions;

        private IRepository<InfluencerMedia> _InfluencerMedia;
        private IRepository<InfluencerAsset> _InfluencerAssets;

        private IRepository<BrandProfile> _BrandProfiles;
        private IRepository<AgencyProfile> _AgencyProfiles;
        private IRepository<AgencyClient> _AgencyClients;
        private IRepository<InfluencerBusiness> _InfluencerBusinesses;

        public UnitOfWork(AppDBContext context)
        {
            _context = context;
        }

        public IRepository<MetaPages> MetaPages => _MetaPages ??= new Repository<MetaPages>(_context);
        public IRepository<KeyWords> KeyWords => _KeyWords ??= new Repository<KeyWords>(_context);
        public IRepository<Pixels> Pixels => _Pixels ??= new Repository<Pixels>(_context);

        public IRepository<UserProfile> UserProfiles => _UserProfiles ??= new Repository<UserProfile>(_context);
        public IRepository<InfluencerProfile> InfluencerProfiles => _InfluencerProfiles ??= new Repository<InfluencerProfile>(_context);
        public IRepository<SocialAccount> SocialAccounts => _SocialAccounts ??= new Repository<SocialAccount>(_context);

        public IRepository<Niche> Niches => _Niches ??= new Repository<Niche>(_context);
        public IRepository<UserNiche> UserNiches => _UserNiches ??= new Repository<UserNiche>(_context);

        public IRepository<UserKeyWord> UserKeyWords => _UserKeyWords ??= new Repository<UserKeyWord>(_context);
        public IRepository<VerificationRequest> VerificationRequests => _VerificationRequests ??= new Repository<VerificationRequest>(_context);

        public IRepository<ServiceListing> ServiceListings => _ServiceListings ??= new Repository<ServiceListing>(_context);
        public IRepository<ServicePricingOption> ServicePricingOptions => _ServicePricingOptions ??= new Repository<ServicePricingOption>(_context);

        public IRepository<InfluencerMedia> InfluencerMedia => _InfluencerMedia ??= new Repository<InfluencerMedia>(_context);
        public IRepository<InfluencerAsset> InfluencerAssets => _InfluencerAssets ??= new Repository<InfluencerAsset>(_context);


        public IRepository<BrandProfile> BrandProfiles => _BrandProfiles ??= new Repository<BrandProfile>(_context);
        public IRepository<AgencyProfile> AgencyProfiles => _AgencyProfiles ??= new Repository<AgencyProfile>(_context);
        public IRepository<AgencyClient> AgencyClients => _AgencyClients ??= new Repository<AgencyClient>(_context);
        public IRepository<InfluencerBusiness> InfluencerBusinesses => _InfluencerBusinesses ??= new Repository<InfluencerBusiness>(_context);

        public async Task<int> CompleteAsync() => await _context.SaveChangesAsync();
        public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();

        public void Dispose() => _context.Dispose();
    }
}
