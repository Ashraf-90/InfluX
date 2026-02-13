using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class ServiceListingDto : CommonDto
    {
        public int Id { get; set; }
        public int InfluencerId { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public SocialPlatform Platform { get; set; }
        public DeliverableType DeliverableType { get; set; }
        public decimal BasePrice { get; set; }
        public int DurationDays { get; set; }
        public int RevisionsCount { get; set; }
        public ListingStatus Status { get; set; }
    }

    public class ServiceListingCreateDto : CommonCreateDto
    {
        public int InfluencerId { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public SocialPlatform Platform { get; set; }
        public DeliverableType DeliverableType { get; set; }
        public decimal BasePrice { get; set; }
        public int DurationDays { get; set; }
        public int RevisionsCount { get; set; }
    }

    public class ServiceListingUpdateDto : CommonCreateDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public decimal BasePrice { get; set; }
        public int DurationDays { get; set; }
        public int RevisionsCount { get; set; }
        public ListingStatus Status { get; set; }
    }

    public class ServiceListingDeleteDto { public int Id { get; set; } }
}

