using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.DTOs
{
    public class DisputeDto : CommonDto
    {
        public Guid OrderId { get; set; }
        public Guid OpenedBy { get; set; }
        public string Reason { get; set; } = null!;
        public DisputeStatus Status { get; set; }
        public string? ResolutionNotes { get; set; }
    }

    public class DisputeCreateDto : CommonCreateDto
    {
        public Guid OrderId { get; set; }
        public Guid OpenedBy { get; set; }
        public string Reason { get; set; } = null!;
        public DisputeStatus Status { get; set; } = DisputeStatus.Open;
        public string? ResolutionNotes { get; set; }
    }

    public class DisputeUpdateDto : CommonCreateDto
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public Guid OpenedBy { get; set; }
        public string Reason { get; set; } = null!;
        public DisputeStatus Status { get; set; }
        public string? ResolutionNotes { get; set; }
    }
}
