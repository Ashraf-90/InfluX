using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class ServicePricingOptionDto : CommonDto
    {
        public int Id { get; set; }
        public int ServiceListingId { get; set; }
        public string Key { get; set; } = null!;
        public decimal Price { get; set; }
        public string? Notes { get; set; }
    }

    public class ServicePricingOptionCreateDto : CommonCreateDto
    {
        public int ServiceListingId { get; set; }
        public string Key { get; set; } = null!;
        public decimal Price { get; set; }
        public string? Notes { get; set; }
    }

    public class ServicePricingOptionUpdateDto : CommonCreateDto
    {
        public int Id { get; set; }
        public string Key { get; set; } = null!;
        public decimal Price { get; set; }
        public string? Notes { get; set; }
    }

    public class ServicePricingOptionDeleteDto { public int Id { get; set; } }
}

