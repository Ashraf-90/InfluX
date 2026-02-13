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
    public class UserKeyWordServices : BaseCrudServices<UserKeyWord, UserKeyWordDto, UserKeyWordCreateDto, UserKeyWordUpdateDto>, IUserKeyWordServices
    {
        public UserKeyWordServices(IUnitOfWork uow, IMapper mapper)
            : base(uow, mapper, uow.UserKeyWords) { }

        protected override Guid GetUpdateId(UserKeyWordUpdateDto dto) => dto.Id;
    }
}


