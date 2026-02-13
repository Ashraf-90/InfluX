using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using AutoMapper;
using Domain.Abstractions;
using Domain.Entities;

namespace Application.Interfaces
{
    public class InfluencerProfileServices : BaseCrudServices<InfluencerProfile, InfluencerProfileDto, InfluencerProfileCreateDto, InfluencerProfileUpdateDto>, IInfluencerProfileServices
    {
        public InfluencerProfileServices(IUnitOfWork uow, IMapper mapper)
            : base(uow, mapper, uow.InfluencerProfiles) { }

        protected override Guid GetUpdateId(InfluencerProfileUpdateDto dto) => dto.Id;
    }
}


