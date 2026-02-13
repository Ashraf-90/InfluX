using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class UserNiche : Common
    {
        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; } = null!;

        public Guid NicheId { get; set; }
        public Niche Niche { get; set; } = null!;
    }
}

