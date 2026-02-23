using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class InfluencerBusiness : Common
    {
        public Guid InfluencerId { get; set; }
        public ApplicationUser Influencer { get; set; } = null!;

        public string Name { get; set; } = null!;
        public string? Logo { get; set; }
        public string? Description { get; set; }

        public BusinessType BusinessType { get; set; } = BusinessType.Company;

        // نخزنها كـ JSON array string مثل:
        // ["facebook","instagram","tiktok"]
        public string? SocialAccountsJson { get; set; }
    }
}
