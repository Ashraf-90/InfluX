using Domain.Entities.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class InfluencerAsset : Common
    {
        public int Id { get; set; }

        public int InfluencerId { get; set; }
        public ApplicationUser Influencer { get; set; } = null!;

        public string Name { get; set; } = null!;
        public string? Description { get; set; }

        public AssetType AssetType { get; set; }
        public string Url { get; set; } = null!;

        public decimal RetailPrice { get; set; }
    }
}

