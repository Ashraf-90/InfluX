using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Campaign : Common
    {
        // Required always
        public Guid BrandProfileId { get; set; }
        public BrandProfile BrandProfile { get; set; } = null!;

        // Null when campaign is created directly by Brand
        public Guid? AgencyProfileId { get; set; }
        public AgencyProfile? AgencyProfile { get; set; }

        public string Title { get; set; } = null!;
        public string? Objective { get; set; }
        public decimal TotalBudget { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public CampaignStatus Status { get; set; } = CampaignStatus.Draft;

        public CampaignRequirement? CampaignRequirement { get; set; }
        public ICollection<CampaignInvite> CampaignInvites { get; set; } = new List<CampaignInvite>();
    }
}
