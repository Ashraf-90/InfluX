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
    public class UserNicheServices : BaseCrudServices<UserNiche, UserNicheDto, UserNicheCreateDto, UserNicheCreateDto>, IUserNicheServices
    {
        public UserNicheServices(IUnitOfWork uow, IMapper mapper)
            : base(uow, mapper, uow.UserNiches) { }

        protected override int EFId(UserNiche entity) => entity.Id;
        protected override int GetUpdateId(UserNicheCreateDto dto) => 0; // Not used (we use same dto)
    }
}

