using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.DTOs
{
    public class OrderApprovalDto : CommonDto
    {
        public Guid OrderId { get; set; }
        public Guid ApprovedBy { get; set; }
        public OrderApprovalStatus Status { get; set; }
        public string? Feedback { get; set; }
    }

    public class OrderApprovalCreateDto : CommonCreateDto
    {
        public Guid OrderId { get; set; }
        public Guid ApprovedBy { get; set; }
        public OrderApprovalStatus Status { get; set; }
        public string? Feedback { get; set; }
    }

    public class OrderApprovalUpdateDto : CommonCreateDto
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public Guid ApprovedBy { get; set; }
        public OrderApprovalStatus Status { get; set; }
        public string? Feedback { get; set; }
    }
}
