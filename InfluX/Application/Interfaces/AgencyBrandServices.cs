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
    public class AgencyBrandServices
        : BaseCrudServices<AgencyBrand, AgencyBrandDto, AgencyBrandCreateDto, AgencyBrandUpdateDto>,
          IAgencyBrandServices
    {
        public AgencyBrandServices(IUnitOfWork uow, IMapper mapper)
            : base(uow, mapper, uow.AgencyBrands)
        {
        }

        protected override Guid GetUpdateId(AgencyBrandUpdateDto dto) => dto.Id;
    }
}
