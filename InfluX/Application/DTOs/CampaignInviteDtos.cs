using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.DTOs
{
    public class CampaignInviteDto : CommonDto
    {
        public Guid CampaignId { get; set; }
        public Guid InfluencerId { get; set; }

        public CampaignInviteStatus Status { get; set; }
    }

    public class CampaignInviteCreateDto : CommonCreateDto
    {
        public Guid CampaignId { get; set; }
        public Guid InfluencerId { get; set; }

        public CampaignInviteStatus Status { get; set; } = CampaignInviteStatus.Sent;
    }

    public class CampaignInviteUpdateDto : CommonCreateDto
    {
        public Guid Id { get; set; }

        public Guid CampaignId { get; set; }
        public Guid InfluencerId { get; set; }

        public CampaignInviteStatus Status { get; set; }
    }
}
