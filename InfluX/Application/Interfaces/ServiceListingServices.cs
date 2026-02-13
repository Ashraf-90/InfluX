using Application.DTOs;
using AutoMapper;
using Domain.Abstractions;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public class ServiceListingServices : BaseCrudServices<ServiceListing, ServiceListingDto, ServiceListingCreateDto, ServiceListingUpdateDto>, IServiceListingServices
    {
        public ServiceListingServices(IUnitOfWork uow, IMapper mapper)
            : base(uow, mapper, uow.ServiceListings) { }

        protected override Guid GetUpdateId(ServiceListingUpdateDto dto) => dto.Id;
    }
}


