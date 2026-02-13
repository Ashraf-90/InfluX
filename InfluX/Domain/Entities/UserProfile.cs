using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class UserProfile : Common
    {
        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; } = null!;

        public string? FullName { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? Language { get; set; }
        public string? AvatarUrl { get; set; }
    }
}

