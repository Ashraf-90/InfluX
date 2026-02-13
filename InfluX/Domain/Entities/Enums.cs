using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
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
    }

}
