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
    public class AgencyClientServices
        : BaseCrudServices<AgencyClient, AgencyClientDto, AgencyClientCreateDto, AgencyClientUpdateDto>,
          IAgencyClientServices
    {
        public AgencyClientServices(IUnitOfWork uow, IMapper mapper)
            : base(uow, mapper, uow.AgencyClients) { }

        protected override Guid GetUpdateId(AgencyClientUpdateDto dto) => dto.Id;
    }
}
