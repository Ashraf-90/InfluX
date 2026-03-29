using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public enum SocialPlatform
    {
        Instagram = 1,
        TikTok = 2,
        Snapchat = 3,
        YouTube = 4,
        X = 5
    }

    public enum VerificationStatus
    {
        Pending = 1,
        Approved = 2,
        Rejected = 3
    }

    public enum DeliverableType
    {
        Post = 1,
        Story = 2,
        Reel = 3,
        Video = 4,
        Live = 5
    }

    public enum ListingStatus
    {
        Active = 1,
        Inactive = 2
    }

    public enum MediaType
    {
        Image = 1,
        Video = 2
    }

    public enum AssetType
    {
        Pdf = 1,
        Image = 2,
        Video = 3,
        Audio = 4,
        Url = 5
    }


    public enum AgencyClientRole
    {
        Owner = 1,
        Manager = 2
    }

    public enum AgencyClientStatus
    {
        Active = 1,
        Inactive = 2
    }

    public enum BusinessType
    {
        Agency = 1,
        Company = 2
    }

    public enum CampaignStatus
    {
        Draft = 1,
        Open = 2,
        InProgress = 3,
        Completed = 4,
        Cancelled = 5
    }

    public enum CampaignInviteStatus
    {
        Sent = 1,
        Accepted = 2,
        Declined = 3
    }

    public enum OrderStatus
    {
        Pending = 1,
        Accepted = 2,
        InProduction = 3,
        Submitted = 4,
        Approved = 5,
        Rejected = 6,
        Cancelled = 7
    }

    public enum OrderDeliverableType
    {
        File = 1,
        Url = 2,
        Text = 3
    }

    public enum OrderApprovalStatus
    {
        Approved = 1,
        Rejected = 2
    }

    public enum DisputeStatus
    {
        Open = 1,
        InReview = 2,
        Resolved = 3,
        Refunded = 4
    }
}

