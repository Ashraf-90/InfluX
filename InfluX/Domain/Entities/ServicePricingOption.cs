using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ServicePricingOption : Common
    {
        public Guid ServiceListingId { get; set; }
        public ServiceListing ServiceListing { get; set; } = null!;

        public string Key { get; set; } = null!;
        public decimal Price { get; set; }
        public string? Notes { get; set; }
    }
}


