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
    public class CampaignRequirementServices
        : BaseCrudServices<CampaignRequirement, CampaignRequirementDto, CampaignRequirementCreateDto, CampaignRequirementUpdateDto>,
          ICampaignRequirementServices
    {
        public CampaignRequirementServices(IUnitOfWork uow, IMapper mapper)
            : base(uow, mapper, uow.CampaignRequirements) { }

        protected override Guid GetUpdateId(CampaignRequirementUpdateDto dto) => dto.Id;
    }
}
