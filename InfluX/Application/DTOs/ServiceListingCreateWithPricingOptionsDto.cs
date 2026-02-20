using System;
using System.Collections.Generic;
using Domain.Entities;

namespace Application.DTOs
{
    public class ServiceListingCreateWithPricingOptionsDto
    {
        // ServiceListing fields
        public string Title { get; set; } = null!;
        public string? Description { get; set; }

        public SocialPlatform Platform { get; set; }
        public DeliverableType DeliverableType { get; set; }

        public decimal BasePrice { get; set; }
        public int DurationDays { get; set; }
        public int RevisionsCount { get; set; }

        // Optional array of pricing options
        public List<ServicePricingOptionCreateItemDto>? PricingOptions { get; set; }
    }

    public class ServicePricingOptionCreateItemDto
    {
        public string Key { get; set; } = null!;
        public decimal Price { get; set; }
        public string? Notes { get; set; }
    }
}