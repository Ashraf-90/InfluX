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
    public class DisputeServices
        : BaseCrudServices<Dispute, DisputeDto, DisputeCreateDto, DisputeUpdateDto>,
          IDisputeServices
    {
        public DisputeServices(IUnitOfWork uow, IMapper mapper)
            : base(uow, mapper, uow.Disputes) { }

        protected override Guid GetUpdateId(DisputeUpdateDto dto) => dto.Id;
    }
}
