using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class InfluencerProfileDto : CommonDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; } = null!;
        public string? Bio { get; set; }
        public string? Gender { get; set; }
        public DateTime? Birthdate { get; set; }
        public bool IsVerified { get; set; }
        public string? PublicSlug { get; set; }
    }

    public class InfluencerProfileCreateDto : CommonCreateDto
    {
        public int UserId { get; set; }
        public string Username { get; set; } = null!;
        public string? Bio { get; set; }
        public string? Gender { get; set; }
        public DateTime? Birthdate { get; set; }
        public string? PublicSlug { get; set; }
    }

    public class InfluencerProfileUpdateDto : CommonCreateDto
    {
        public int Id { get; set; }
        public string? Bio { get; set; }
        public string? Gender { get; set; }
        public DateTime? Birthdate { get; set; }
        public bool IsVerified { get; set; }
        public string? PublicSlug { get; set; }
    }

    public class InfluencerProfileDeleteDto { public int Id { get; set; } }
}

