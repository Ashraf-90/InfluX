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
    public class SocialAccountServices : BaseCrudServices<SocialAccount, SocialAccountDto, SocialAccountCreateDto, SocialAccountUpdateDto>, ISocialAccountServices
    {
        public SocialAccountServices(IUnitOfWork uow, IMapper mapper)
            : base(uow, mapper, uow.SocialAccounts) { }

        protected override Guid GetUpdateId(SocialAccountUpdateDto dto) => dto.Id;
    }
}


