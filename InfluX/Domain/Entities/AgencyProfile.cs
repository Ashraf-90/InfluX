using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class AgencyProfile : Common
    {
        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; } = null!;

        public string AgencyName { get; set; } = null!;
        public string? Website { get; set; }
        public string? Description { get; set; }
        public string? LogoUrl { get; set; }

        public ICollection<AgencyClient> AgencyClients { get; set; } = new List<AgencyClient>();

        // NEW
        public ICollection<AgencyBrand> AgencyBrands { get; set; } = new List<AgencyBrand>();
    }
}
