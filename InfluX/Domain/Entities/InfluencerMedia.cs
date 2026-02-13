using Domain.Entities.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class InfluencerMedia : Common
    {
        public int Id { get; set; }

        public int InfluencerId { get; set; }
        public ApplicationUser Influencer { get; set; } = null!;

        public MediaType Type { get; set; }
        public SocialPlatform Platform { get; set; }

        public string Url { get; set; } = null!;
        public string? Caption { get; set; }
    }
}

