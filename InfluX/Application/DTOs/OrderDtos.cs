using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Domain.Entities;

namespace Application.DTOs
{
    public class OrderDto : CommonDto
    {
        public Guid? CampaignId { get; set; }
        public Guid BuyerId { get; set; }
        public Guid InfluencerId { get; set; }
        public Guid? ServiceListingId { get; set; }

        public string Title { get; set; } = null!;
        public decimal AgreedPrice { get; set; }
        public string? Platform { get; set; }

        public OrderStatus Status { get; set; }
    }

    public class OrderCreateDto : CommonCreateDto
    {
        public Guid? CampaignId { get; set; }
        public Guid BuyerId { get; set; }
        public Guid InfluencerId { get; set; }
        public Guid? ServiceListingId { get; set; }

        public string Title { get; set; } = null!;
        public decimal AgreedPrice { get; set; }
        public string? Platform { get; set; }

        public OrderStatus Status { get; set; } = OrderStatus.Pending;
    }

    public class OrderUpdateDto : CommonCreateDto
    {
        public Guid Id { get; set; }

        public Guid? CampaignId { get; set; }
        public Guid BuyerId { get; set; }
        public Guid InfluencerId { get; set; }
        public Guid? ServiceListingId { get; set; }

        public string Title { get; set; } = null!;
        public decimal AgreedPrice { get; set; }
        public string? Platform { get; set; }

        public OrderStatus Status { get; set; }
    }
}
