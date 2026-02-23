using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Domain.Entities;

namespace Application.DTOs
{
    public class InfluencerBusinessDto : CommonDto
    {
        public Guid InfluencerId { get; set; }

        public string Name { get; set; } = null!;
        public string? Logo { get; set; }
        public string? Description { get; set; }

        public BusinessType BusinessType { get; set; }
        public string? SocialAccountsJson { get; set; }
    }

    public class InfluencerBusinessCreateDto : CommonCreateDto
    {
        public Guid InfluencerId { get; set; } // من التوكن

        public string Name { get; set; } = null!;
        public string? Logo { get; set; }
        public string? Description { get; set; }

        public BusinessType BusinessType { get; set; } = BusinessType.Company;
        public string? SocialAccountsJson { get; set; }
    }

    public class InfluencerBusinessUpdateDto : CommonCreateDto
    {
        public Guid Id { get; set; }
        public Guid InfluencerId { get; set; } // تثبيت الملكية

        public string Name { get; set; } = null!;
        public string? Logo { get; set; }
        public string? Description { get; set; }

        public BusinessType BusinessType { get; set; }
        public string? SocialAccountsJson { get; set; }
    }
}
