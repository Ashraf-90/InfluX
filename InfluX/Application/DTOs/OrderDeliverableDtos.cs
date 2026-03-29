using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.DTOs
{
    public class OrderDeliverableDto : CommonDto
    {
        public Guid OrderId { get; set; }
        public OrderDeliverableType Type { get; set; }
        public string UrlOrText { get; set; } = null!;
        public DateTime SubmittedAt { get; set; }
    }

    public class OrderDeliverableCreateDto : CommonCreateDto
    {
        public Guid OrderId { get; set; }
        public OrderDeliverableType Type { get; set; }
        public string UrlOrText { get; set; } = null!;
        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
    }

    public class OrderDeliverableUpdateDto : CommonCreateDto
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public OrderDeliverableType Type { get; set; }
        public string UrlOrText { get; set; } = null!;
        public DateTime SubmittedAt { get; set; }
    }
}
