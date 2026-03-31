using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class OrderApproval : Common
    {
        public Guid OrderId { get; set; }
        public Order Order { get; set; } = null!;

        public Guid ApprovedBy { get; set; }
        public ApplicationUser ApprovedByUser { get; set; } = null!;

        public OrderApprovalStatus Status { get; set; }

        public string? Feedback { get; set; }
    }
}
