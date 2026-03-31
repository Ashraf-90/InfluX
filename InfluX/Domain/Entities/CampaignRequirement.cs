using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class CampaignRequirement : Common
    {
        public Guid CampaignId { get; set; }
        public Campaign Campaign { get; set; } = null!;

        // نخزنها كنص مفصول بفواصل لتبقى متوافقة مع SQL Server بسهولة
        public string? Platforms { get; set; }

        public string? ContentGuidelines { get; set; }
        public string? Hashtags { get; set; }
        public string? Mentions { get; set; }
        public string? TargetAudience { get; set; }
        public string? Notes { get; set; }
    }
}
