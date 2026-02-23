using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;

namespace Application.DTOs
{
    public class BrandProfileDto : CommonDto
    {
        public Guid UserId { get; set; }

        public string BrandName { get; set; } = null!;
        public string? Website { get; set; }
        public string? Description { get; set; }
        public string? LogoUrl { get; set; }

        public string? Industry { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
    }

    public class BrandProfileCreateDto : CommonCreateDto
    {
        public Guid UserId { get; set; } // سيتم تثبيته من التوكن في الـ API

        public string BrandName { get; set; } = null!;
        public string? Website { get; set; }
        public string? Description { get; set; }
        public string? LogoUrl { get; set; }

        public string? Industry { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
    }

    public class BrandProfileUpdateDto : CommonCreateDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; } // تثبيت الملكية

        public string BrandName { get; set; } = null!;
        public string? Website { get; set; }
        public string? Description { get; set; }
        public string? LogoUrl { get; set; }

        public string? Industry { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
    }
}
