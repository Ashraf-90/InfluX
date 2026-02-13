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
    public class UserKeyWordServices : BaseCrudServices<UserKeyWord, UserKeyWordDto, UserKeyWordCreateDto, UserKeyWordCreateDto>, IUserKeyWordServices
    {
        public UserKeyWordServices(IUnitOfWork uow, IMapper mapper)
            : base(uow, mapper, uow.UserKeyWords) { }

        protected override int EFId(UserKeyWord entity) => entity.Id;
        protected override int GetUpdateId(UserKeyWordCreateDto dto) => 0;
    }
}

