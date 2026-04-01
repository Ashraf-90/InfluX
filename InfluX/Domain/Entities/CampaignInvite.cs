using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class CampaignInvite : Common
    {
        public Guid CampaignId { get; set; }
        public Campaign Campaign { get; set; } = null!;

        public Guid InfluencerId { get; set; }

        [InverseProperty(nameof(ApplicationUser.CampaignInvites))]
        public ApplicationUser Influencer { get; set; } = null!;

        public CampaignInviteStatus Status { get; set; } = CampaignInviteStatus.Sent;
    }
}
