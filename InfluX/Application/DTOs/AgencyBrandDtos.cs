using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;

namespace Application.DTOs
{
    public class AgencyBrandDto : CommonDto
    {
        public Guid AgencyId { get; set; }
        public Guid BrandId { get; set; }

        public string? AgencyName { get; set; }
        public string? BrandName { get; set; }
        public string? Website { get; set; }
        public string? LogoUrl { get; set; }
        public string? Industry { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
    }

    public class AgencyBrandCreateDto : CommonCreateDto
    {
        public Guid AgencyId { get; set; }
        public Guid BrandId { get; set; }
    }

    public class AgencyBrandUpdateDto : CommonCreateDto
    {
        public Guid Id { get; set; }
        public Guid AgencyId { get; set; }
        public Guid BrandId { get; set; }
    }

    public class AttachExistingBrandToAgencyDto
    {
        public Guid BrandId { get; set; } // BrandProfile.Id
    }

    public class CreateBrandAndAttachDto
    {
        // بيانات اليوزر الجديد الخاص بالبراند
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? PhoneNumber { get; set; }

        // بيانات UserProfile
        public string? FullName { get; set; }
        public string? Language { get; set; }
        public string? AvatarUrl { get; set; }

        // بيانات BrandProfile
        public string BrandName { get; set; } = null!;
        public string? Website { get; set; }
        public string? Description { get; set; }
        public string? LogoUrl { get; set; }
        public string? Industry { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
    }
}
