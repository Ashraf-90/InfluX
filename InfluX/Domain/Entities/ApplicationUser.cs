using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    // Identity user MUST inherit IdentityUser<TKey>
    public class ApplicationUser : IdentityUser<int>
    {
        // ---- "Common" fields (same behavior as Common) ----
        public bool Active { get; set; } = true;
        public bool IsAvilable { get; set; } = true;
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }

        // ---- Business role (separate from IdentityRole) ----
        // This is your ERD role: Brand/Influencer/Agency/Admin/User
        public string AppRole { get; set; } = "User";

        // Navigation
        public UserProfile? UserProfile { get; set; }
        public InfluencerProfile? InfluencerProfile { get; set; }

        public ICollection<SocialAccount> SocialAccounts { get; set; } = new List<SocialAccount>();
        public ICollection<UserNiche> UserNiches { get; set; } = new List<UserNiche>();
        public ICollection<UserKeyWord> UserKeyWords { get; set; } = new List<UserKeyWord>();
        public ICollection<VerificationRequest> VerificationRequests { get; set; } = new List<VerificationRequest>();

        public ICollection<ServiceListing> ServiceListings { get; set; } = new List<ServiceListing>();
        public ICollection<InfluencerMedia> InfluencerMedia { get; set; } = new List<InfluencerMedia>();
        public ICollection<InfluencerAsset> InfluencerAssets { get; set; } = new List<InfluencerAsset>();
    }
}

