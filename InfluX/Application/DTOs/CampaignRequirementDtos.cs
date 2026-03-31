using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class CampaignRequirementDto : CommonDto
    {
        public Guid CampaignId { get; set; }

        public string? Platforms { get; set; }
        public string? ContentGuidelines { get; set; }
        public string? Hashtags { get; set; }
        public string? Mentions { get; set; }
        public string? TargetAudience { get; set; }
        public string? Notes { get; set; }
    }

    public class CampaignRequirementCreateDto : CommonCreateDto
    {
        public Guid CampaignId { get; set; }

        public string? Platforms { get; set; }
        public string? ContentGuidelines { get; set; }
        public string? Hashtags { get; set; }
        public string? Mentions { get; set; }
        public string? TargetAudience { get; set; }
        public string? Notes { get; set; }
    }

    public class CampaignRequirementUpdateDto : CommonCreateDto
    {
        public Guid Id { get; set; }
        public Guid CampaignId { get; set; }

        public string? Platforms { get; set; }
        public string? ContentGuidelines { get; set; }
        public string? Hashtags { get; set; }
        public string? Mentions { get; set; }
        public string? TargetAudience { get; set; }
        public string? Notes { get; set; }
    }
}
