using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class KeyWords : Common
    {
        public string? EnKeyword { get; set; }
        public string? ArKeyword { get; set; }

        public ICollection<UserKeyWord> UserKeyWords { get; set; } = new List<UserKeyWord>();
    }
}

