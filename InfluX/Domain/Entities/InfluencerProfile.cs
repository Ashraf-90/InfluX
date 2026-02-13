using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class InfluencerProfile : Common
    {
        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; } = null!;

        public string Username { get; set; } = null!;
        public string? Bio { get; set; }
        public string? Gender { get; set; }
        public DateTime? Birthdate { get; set; }

        public bool IsVerified { get; set; } = false;
        public string? PublicSlug { get; set; }
    }
}

