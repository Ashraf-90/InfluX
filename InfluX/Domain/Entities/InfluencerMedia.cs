using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class InfluencerMedia : Common
    {
        public Guid InfluencerId { get; set; }
        public ApplicationUser Influencer { get; set; } = null!;

        public MediaType Type { get; set; }
        public SocialPlatform Platform { get; set; }

        public string Url { get; set; } = null!;
        public string? Caption { get; set; }
    }
}


