using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class InfluencerMediaDto : CommonDto
    {
        public Guid InfluencerId { get; set; }

        public MediaType Type { get; set; }
        public SocialPlatform Platform { get; set; }

        public string Url { get; set; } = null!;
        public string? Caption { get; set; }
    }

    public class InfluencerMediaCreateDto : CommonCreateDto
    {
        public Guid InfluencerId { get; set; }

        public MediaType Type { get; set; }
        public SocialPlatform Platform { get; set; }

        public string Url { get; set; } = null!;
        public string? Caption { get; set; }
    }

    public class InfluencerMediaUpdateDto : CommonCreateDto
    {
        public Guid Id { get; set; }

        public MediaType Type { get; set; }
        public SocialPlatform Platform { get; set; }

        public string Url { get; set; } = null!;
        public string? Caption { get; set; }
    }

    public class InfluencerMediaDeleteDto
    {
        public Guid Id { get; set; }
    }
}


