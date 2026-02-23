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
    public class AgencyProfileServices
        : BaseCrudServices<AgencyProfile, AgencyProfileDto, AgencyProfileCreateDto, AgencyProfileUpdateDto>,
          IAgencyProfileServices
    {
        public AgencyProfileServices(IUnitOfWork uow, IMapper mapper)
            : base(uow, mapper, uow.AgencyProfiles) { }

        protected override Guid GetUpdateId(AgencyProfileUpdateDto dto) => dto.Id;
    }
}
