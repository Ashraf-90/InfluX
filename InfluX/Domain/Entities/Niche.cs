using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Niche : Common
    {
        public string NameAr { get; set; } = null!;
        public string NameEn { get; set; } = null!;
        public string? Icon { get; set; }

        public ICollection<UserNiche> UserNiches { get; set; } = new List<UserNiche>();
    }
}

