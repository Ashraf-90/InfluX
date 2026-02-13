using Application.DTOs;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Application.Interfaces
{
    public class IdentityUserServices : IIdentityUserServices
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IMapper mapper;

        public IdentityUserServices(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IMapper mapper)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.mapper = mapper;
        }

        public async Task<(bool ok, string message)> Register(IdentityUserCreateDto dto)
        {
            var user = new ApplicationUser
            {
                UserName = dto.UserName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                AppRole = dto.AppRole,
                Active = dto.Active,
                IsAvilable = dto.IsAvilable
            };

            var result = await userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
                return (false, string.Join(" | ", result.Errors.Select(e => e.Description)));

            // Optional: create IdentityRole mapping too (if you want)
            // await userManager.AddToRoleAsync(user, dto.AppRole);

            return (true, "User created successfully");
        }

        public async Task<(bool ok, string message)> Login(LoginDto dto)
        {
            ApplicationUser? user = await userManager.FindByEmailAsync(dto.EmailOrUserName);
            if (user == null)
                user = await userManager.FindByNameAsync(dto.EmailOrUserName);

            if (user == null || !user.Active)
                return (false, "Invalid credentials");

            var result = await signInManager.PasswordSignInAsync(user, dto.Password, dto.RememberMe, lockoutOnFailure: true);
            return result.Succeeded ? (true, "Logged in") : (false, "Invalid credentials");
        }

        public async Task<bool> Logout()
        {
            await signInManager.SignOutAsync();
            return true;
        }

        public async Task<IdentityUserDto?> GetById(int id)
        {
            var user = await userManager.FindByIdAsync(id.ToString());
            if (user == null) return null;
            return mapper.Map<IdentityUserDto>(user);
        }

        public async Task<bool> Update(IdentityUserUpdateDto dto)
        {
            var user = await userManager.FindByIdAsync(dto.Id.ToString());
            if (user == null) return false;

            user.PhoneNumber = dto.PhoneNumber;
            user.AppRole = dto.AppRole;
            user.Active = dto.Active;
            user.IsAvilable = dto.IsAvilable;

            var result = await userManager.UpdateAsync(user);
            return result.Succeeded;
        }

        // SoftDelete => Active=false (Identity rows stay)
        public async Task<bool> SoftDelete(int id)
        {
            var user = await userManager.FindByIdAsync(id.ToString());
            if (user == null) return false;

            user.Active = false;
            var result = await userManager.UpdateAsync(user);
            return result.Succeeded;
        }
    }
}
