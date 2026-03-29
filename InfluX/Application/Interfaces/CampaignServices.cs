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
    public class CampaignServices
        : BaseCrudServices<Campaign, CampaignDto, CampaignCreateDto, CampaignUpdateDto>,
          ICampaignServices
    {
        public CampaignServices(IUnitOfWork uow, IMapper mapper)
            : base(uow, mapper, uow.Campaigns) { }

        protected override Guid GetUpdateId(CampaignUpdateDto dto) => dto.Id;
    }
}
