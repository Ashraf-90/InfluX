using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Application.DTOs;
using AutoMapper;
using Domain.Abstractions;
using Domain.Entities;

namespace Application.Interfaces
{
    public class CampaignInviteServices
        : BaseCrudServices<CampaignInvite, CampaignInviteDto, CampaignInviteCreateDto, CampaignInviteUpdateDto>,
          ICampaignInviteServices
    {
        public CampaignInviteServices(IUnitOfWork uow, IMapper mapper)
            : base(uow, mapper, uow.CampaignInvites) { }

        protected override Guid GetUpdateId(CampaignInviteUpdateDto dto) => dto.Id;
    }
}
