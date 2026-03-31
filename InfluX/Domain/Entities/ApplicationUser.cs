using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        // Common-like fields (IdentityUser<Guid> already has Id)
        public bool Active { get; set; } = true;
        public bool IsAvilable { get; set; } = true;
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }

        public string AppRole { get; set; } = "User";

        public UserProfile? UserProfile { get; set; }
        public InfluencerProfile? InfluencerProfile { get; set; }

        public BrandProfile? BrandProfile { get; set; }
        public AgencyProfile? AgencyProfile { get; set; }

        public ICollection<SocialAccount> SocialAccounts { get; set; } = new List<SocialAccount>();
        public ICollection<UserNiche> UserNiches { get; set; } = new List<UserNiche>();
        public ICollection<UserKeyWord> UserKeyWords { get; set; } = new List<UserKeyWord>();
        public ICollection<VerificationRequest> VerificationRequests { get; set; } = new List<VerificationRequest>();

        public ICollection<ServiceListing> ServiceListings { get; set; } = new List<ServiceListing>();
        public ICollection<InfluencerMedia> InfluencerMedia { get; set; } = new List<InfluencerMedia>();
        public ICollection<InfluencerAsset> InfluencerAssets { get; set; } = new List<InfluencerAsset>();


        public ICollection<InfluencerBusiness> InfluencerBusinesses { get; set; } = new List<InfluencerBusiness>();


        public ICollection<Campaign> BrandCampaigns { get; set; } = new List<Campaign>();
        public ICollection<Campaign> AgencyCampaigns { get; set; } = new List<Campaign>();
        public ICollection<CampaignInvite> CampaignInvites { get; set; } = new List<CampaignInvite>();

        public ICollection<Order> BuyerOrders { get; set; } = new List<Order>();
        public ICollection<Order> InfluencerOrders { get; set; } = new List<Order>();
        public ICollection<OrderApproval> OrderApprovals { get; set; } = new List<OrderApproval>();
        public ICollection<Dispute> OpenedDisputes { get; set; } = new List<Dispute>();
    }
}



