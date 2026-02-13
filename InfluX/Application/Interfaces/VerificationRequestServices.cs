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
    public class VerificationRequestServices : BaseCrudServices<VerificationRequest, VerificationRequestDto, VerificationRequestCreateDto, VerificationRequestUpdateDto>, IVerificationRequestServices
    {
        public VerificationRequestServices(IUnitOfWork uow, IMapper mapper)
            : base(uow, mapper, uow.VerificationRequests) { }

        protected override Guid GetUpdateId(VerificationRequestUpdateDto dto) => dto.Id;
    }
}


