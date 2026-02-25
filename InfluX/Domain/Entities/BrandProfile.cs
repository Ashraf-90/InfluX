using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class BrandProfile : Common
    {
        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; } = null!;

        public string BrandName { get; set; } = null!;
        public string? Website { get; set; }
        public string? Description { get; set; }
        public string? LogoUrl { get; set; }

        public string? Industry { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }

        public ICollection<AgencyClient> AgencyClients { get; set; } = new List<AgencyClient>();
    }
}
