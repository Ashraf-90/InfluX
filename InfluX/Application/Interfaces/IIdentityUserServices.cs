using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;

namespace Application.Interfaces
{
    public interface IIdentityUserServices
    {
        Task<(bool ok, string message)> Register(IdentityUserCreateDto dto);
        Task<(bool ok, string message)> Login(LoginDto dto);
        Task<bool> Logout();

        Task<IdentityUserDto?> GetById(Guid userId);
        Task<bool> Update(IdentityUserUpdateDto dto);
        Task<bool> SoftDelete(Guid userId);
    }
}


