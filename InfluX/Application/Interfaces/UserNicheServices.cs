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
    public class UserNicheServices : BaseCrudServices<UserNiche, UserNicheDto, UserNicheCreateDto, UserNicheUpdateDto>, IUserNicheServices
    {
        public UserNicheServices(IUnitOfWork uow, IMapper mapper)
            : base(uow, mapper, uow.UserNiches) { }

        protected override Guid GetUpdateId(UserNicheUpdateDto dto) => dto.Id;
    }
}


