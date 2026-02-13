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
        public Guid UserId { get; set; } // Identity Id
        public string AppRole { get; set; } = "User";

        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? PhoneNumber { get; set; }
    }

    // REGISTER
    public class IdentityUserCreateDto : CommonCreateDto
    {
        public string AppRole { get; set; } = "User";

        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? PhoneNumber { get; set; }

        public string Password { get; set; } = null!;
    }

    // UPDATE
    public class IdentityUserUpdateDto : CommonCreateDto
    {
        public Guid UserId { get; set; }
        public string AppRole { get; set; } = "User";
        public string? PhoneNumber { get; set; }
    }

    // SOFT DELETE
    public class IdentityUserDeleteDto
    {
        public Guid UserId { get; set; }
    }

    // LOGIN
    public class LoginDto
    {
        public string EmailOrUserName { get; set; } = null!;
        public string Password { get; set; } = null!;
        public bool RememberMe { get; set; } = false;
    }
}


