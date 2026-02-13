using Domain.Entities.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ServiceListing : Common
    {
        public int Id { get; set; }

        public int InfluencerId { get; set; } // ApplicationUser.Id
        public ApplicationUser Influencer { get; set; } = null!;

        public string Title { get; set; } = null!;
        public string? Description { get; set; }

        public SocialPlatform Platform { get; set; }
        public DeliverableType DeliverableType { get; set; }

        public decimal BasePrice { get; set; }
        public int DurationDays { get; set; }
        public int RevisionsCount { get; set; }

        public ListingStatus Status { get; set; } = ListingStatus.Active;

        public ICollection<ServicePricingOption> PricingOptions { get; set; } = new List<ServicePricingOption>();
    }
}

