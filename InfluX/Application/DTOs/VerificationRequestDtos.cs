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
        public Guid UserId { get; set; }

        public VerificationStatus Status { get; set; }

        public string? DocType { get; set; }
        public string? DocUrl { get; set; }
        public string? Notes { get; set; }
    }

    public class VerificationRequestCreateDto : CommonCreateDto
    {
        public Guid UserId { get; set; }

        public string? DocType { get; set; }
        public string? DocUrl { get; set; }
        public string? Notes { get; set; }
    }

    public class VerificationRequestUpdateDto : CommonCreateDto
    {
        public Guid Id { get; set; }

        public VerificationStatus Status { get; set; }
        public string? Notes { get; set; }
    }

    public class VerificationRequestDeleteDto
    {
        public Guid Id { get; set; }
    }
}


