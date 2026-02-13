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
    public class UserProfileServices : BaseCrudServices<UserProfile, UserProfileDto, UserProfileCreateDto, UserProfileUpdateDto>, IUserProfileServices
    {
        public UserProfileServices(IUnitOfWork uow, IMapper mapper)
            : base(uow, mapper, uow.UserProfiles) { }

        protected override int EFId(UserProfile entity) => entity.Id;
        protected override int GetUpdateId(UserProfileUpdateDto dto) => dto.Id;
    }
}

