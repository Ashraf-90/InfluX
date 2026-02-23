using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class AgencyClient : Common
    {
        public Guid AgencyId { get; set; }
        public ApplicationUser Agency { get; set; } = null!;

        public Guid BrandId { get; set; }
        public ApplicationUser Brand { get; set; } = null!;

        public AgencyClientRole Role { get; set; } = AgencyClientRole.Manager;
        public AgencyClientStatus Status { get; set; } = AgencyClientStatus.Active;
    }
}