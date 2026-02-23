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
    public class BrandProfileServices
        : BaseCrudServices<BrandProfile, BrandProfileDto, BrandProfileCreateDto, BrandProfileUpdateDto>,
          IBrandProfileServices
    {
        public BrandProfileServices(IUnitOfWork uow, IMapper mapper)
            : base(uow, mapper, uow.BrandProfiles) { }

        protected override Guid GetUpdateId(BrandProfileUpdateDto dto) => dto.Id;
    }
}
