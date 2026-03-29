using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class OrderDeliverable : Common
    {
        public Guid OrderId { get; set; }
        public Order Order { get; set; } = null!;

        public OrderDeliverableType Type { get; set; }

        public string UrlOrText { get; set; } = null!;

        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
    }
}
