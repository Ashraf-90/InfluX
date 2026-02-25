using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Domain.Entities;

namespace Application.DTOs
{
    public class AgencyClientDto : CommonDto
    {
        public Guid AgencyProfileId { get; set; }
        public Guid BrandProfileId { get; set; }
        public AgencyClientRole Role { get; set; }
        public AgencyClientStatus Status { get; set; }
    }

    public class AgencyClientCreateDto : CommonCreateDto
    {
        public Guid AgencyProfileId { get; set; }
        public Guid BrandProfileId { get; set; }

        public AgencyClientRole Role { get; set; } = AgencyClientRole.Manager;
        public AgencyClientStatus Status { get; set; } = AgencyClientStatus.Active;
    }

    public class AgencyClientUpdateDto : CommonCreateDto
    {
        public Guid Id { get; set; }
        public Guid AgencyProfileId { get; set; }
        public Guid BrandProfileId { get; set; }

        public AgencyClientRole Role { get; set; }
        public AgencyClientStatus Status { get; set; }
    }
}
