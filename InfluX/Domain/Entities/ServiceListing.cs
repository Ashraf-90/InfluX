using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ServiceListing : Common
    {
        public Guid InfluencerId { get; set; }
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
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}

