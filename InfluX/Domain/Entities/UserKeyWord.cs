using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class UserKeyWord : Common
    {
        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; } = null!;

        public Guid KeyWordsId { get; set; }
        public KeyWords KeyWords { get; set; } = null!;
    }
}

