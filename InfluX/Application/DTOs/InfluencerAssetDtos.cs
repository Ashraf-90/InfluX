using Domain.Entities;
using Domain.Entities.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class InfluencerAssetDto : CommonDto
    {
        public int Id { get; set; }
        public int InfluencerId { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public AssetType AssetType { get; set; }
        public string Url { get; set; } = null!;
        public decimal RetailPrice { get; set; }
    }

    public class InfluencerAssetCreateDto : CommonCreateDto
    {
        public int InfluencerId { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public AssetType AssetType { get; set; }
        public string Url { get; set; } = null!;
        public decimal RetailPrice { get; set; }
    }

    public class InfluencerAssetUpdateDto : CommonCreateDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string Url { get; set; } = null!;
        public decimal RetailPrice { get; set; }
    }

    public class InfluencerAssetDeleteDto { public int Id { get; set; } }
}

