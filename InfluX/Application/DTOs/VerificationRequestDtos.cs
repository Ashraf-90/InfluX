using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class VerificationRequestDto : CommonDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public VerificationStatus Status { get; set; }
        public string? DocType { get; set; }
        public string? DocUrl { get; set; }
        public string? Notes { get; set; }
    }

    public class VerificationRequestCreateDto : CommonCreateDto
    {
        public int UserId { get; set; }
        public string? DocType { get; set; }
        public string? DocUrl { get; set; }
        public string? Notes { get; set; }
    }

    public class VerificationRequestUpdateDto : CommonCreateDto
    {
        public int Id { get; set; }
        public VerificationStatus Status { get; set; }
        public string? Notes { get; set; }
    }

    public class VerificationRequestDeleteDto { public int Id { get; set; } }
}

