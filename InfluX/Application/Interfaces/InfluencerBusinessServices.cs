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
    public class InfluencerBusinessServices
        : BaseCrudServices<InfluencerBusiness, InfluencerBusinessDto, InfluencerBusinessCreateDto, InfluencerBusinessUpdateDto>,
          IInfluencerBusinessServices
    {
        public InfluencerBusinessServices(IUnitOfWork uow, IMapper mapper)
            : base(uow, mapper, uow.InfluencerBusinesses) { }

        protected override Guid GetUpdateId(InfluencerBusinessUpdateDto dto) => dto.Id;
    }
}
