using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Order : Common
    {
        public Guid? CampaignId { get; set; }
        public Campaign? Campaign { get; set; }

        public Guid BuyerId { get; set; }

        [InverseProperty(nameof(ApplicationUser.BuyerOrders))]
        public ApplicationUser Buyer { get; set; } = null!;

        public Guid InfluencerId { get; set; }

        [InverseProperty(nameof(ApplicationUser.InfluencerOrders))]
        public ApplicationUser Influencer { get; set; } = null!;

        public Guid? ServiceListingId { get; set; }
        public ServiceListing? ServiceListing { get; set; }

        public string Title { get; set; } = null!;
        public decimal AgreedPrice { get; set; }
        public string? Platform { get; set; }

        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        public ICollection<OrderDeliverable> OrderDeliverables { get; set; } = new List<OrderDeliverable>();
        public ICollection<OrderApproval> OrderApprovals { get; set; } = new List<OrderApproval>();
        public ICollection<Dispute> Disputes { get; set; } = new List<Dispute>();
    }
}