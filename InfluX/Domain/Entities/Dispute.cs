using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Dispute : Common
    {
        public Guid OrderId { get; set; }
        public Order Order { get; set; } = null!;

        public Guid OpenedBy { get; set; }

        [InverseProperty(nameof(ApplicationUser.OpenedDisputes))]
        public ApplicationUser OpenedByUser { get; set; } = null!;

        public string Reason { get; set; } = null!;
        public DisputeStatus Status { get; set; } = DisputeStatus.Open;
        public string? ResolutionNotes { get; set; }
    }
}
