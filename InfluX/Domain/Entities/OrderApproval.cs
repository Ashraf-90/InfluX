using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class OrderApproval : Common
    {
        public Guid OrderId { get; set; }
        public Order Order { get; set; } = null!;

        public Guid ApprovedBy { get; set; }

        [InverseProperty(nameof(ApplicationUser.OrderApprovals))]
        public ApplicationUser ApprovedByUser { get; set; } = null!;

        public OrderApprovalStatus Status { get; set; }
        public string? Feedback { get; set; }
    }
}
