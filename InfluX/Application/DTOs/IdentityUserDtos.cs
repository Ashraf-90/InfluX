using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    // READ
    public class IdentityUserDto : CommonDto
    {
        public int Id { get; set; }
        public string AppRole { get; set; } = "User";
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? PhoneNumber { get; set; }
    }

    // CREATE (Register)
    public class IdentityUserCreateDto : CommonCreateDto
    {
        public string AppRole { get; set; } = "User";
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public string Password { get; set; } = null!;
    }

    // UPDATE (Profile update)
    public class IdentityUserUpdateDto : CommonCreateDto
    {
        public int Id { get; set; }
        public string AppRole { get; set; } = "User";
        public string? PhoneNumber { get; set; }
    }

    // SOFT DELETE
    public class IdentityUserDeleteDto
    {
        public int Id { get; set; }
    }

    // LOGIN
    public class LoginDto
    {
        public string EmailOrUserName { get; set; } = null!;
        public string Password { get; set; } = null!;
        public bool RememberMe { get; set; } = false;
    }
}

