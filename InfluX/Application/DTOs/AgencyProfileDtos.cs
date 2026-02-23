using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;

namespace Application.DTOs
{
    public class AgencyProfileDto : CommonDto
    {
        public Guid UserId { get; set; }

        public string AgencyName { get; set; } = null!;
        public string? Website { get; set; }
        public string? Description { get; set; }
        public string? LogoUrl { get; set; }
    }

    public class AgencyProfileCreateDto : CommonCreateDto
    {
        public Guid UserId { get; set; } // من التوكن

        public string AgencyName { get; set; } = null!;
        public string? Website { get; set; }
        public string? Description { get; set; }
        public string? LogoUrl { get; set; }
    }

    public class AgencyProfileUpdateDto : CommonCreateDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }

        public string AgencyName { get; set; } = null!;
        public string? Website { get; set; }
        public string? Description { get; set; }
        public string? LogoUrl { get; set; }
    }
}
