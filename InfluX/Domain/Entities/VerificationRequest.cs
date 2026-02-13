using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    namespace Domain.Entities
    {
        public class VerificationRequest : Common
        {
            public int Id { get; set; }

            public int UserId { get; set; }
            public ApplicationUser User { get; set; } = null!;

            public VerificationStatus Status { get; set; } = VerificationStatus.Pending;

            public string? DocType { get; set; }
            public string? DocUrl { get; set; }
            public string? Notes { get; set; }
        }
    }

}
