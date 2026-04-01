using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class AgencyBrand : Common
    {
        // FK -> AgencyProfiles.Id
        public Guid AgencyId { get; set; }
        public AgencyProfile Agency { get; set; } = null!;

        // FK -> BrandProfiles.Id
        public Guid BrandId { get; set; }
        public BrandProfile Brand { get; set; } = null!;
    }
}
