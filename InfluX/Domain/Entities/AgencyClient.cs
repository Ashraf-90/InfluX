using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;

namespace Domain.Entities
{
    public class AgencyClient : Common
    {
        // FK -> AgencyProfiles
        public Guid AgencyProfileId { get; set; }
        public AgencyProfile AgencyProfile { get; set; } = null!;

        // FK -> BrandProfiles
        public Guid BrandProfileId { get; set; }
        public BrandProfile BrandProfile { get; set; } = null!;

        public AgencyClientRole Role { get; set; } = AgencyClientRole.Manager;
        public AgencyClientStatus Status { get; set; } = AgencyClientStatus.Active;
    }
}