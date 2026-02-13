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
    public class ServicePricingOptionServices : BaseCrudServices<ServicePricingOption, ServicePricingOptionDto, ServicePricingOptionCreateDto, ServicePricingOptionUpdateDto>, IServicePricingOptionServices
    {
        public ServicePricingOptionServices(IUnitOfWork uow, IMapper mapper)
            : base(uow, mapper, uow.ServicePricingOptions) { }

        protected override Guid GetUpdateId(ServicePricingOptionUpdateDto dto) => dto.Id;
    }
}


