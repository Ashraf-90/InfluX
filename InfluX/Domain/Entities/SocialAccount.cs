using Domain.Entities.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class SocialAccount : Common
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public ApplicationUser User { get; set; } = null!;

        public SocialPlatform Platform { get; set; }
        public string? Handle { get; set; }
        public string? ProfileUrl { get; set; }

        public int Followers { get; set; }
        public int AvgViews { get; set; }
        public decimal EngagementRate { get; set; }

        public bool IsConnected { get; set; } = false;
    }
}
