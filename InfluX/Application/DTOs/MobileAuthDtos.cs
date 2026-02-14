using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    // =========================
    // Unified Response
    // =========================
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = "";
        public T? Data { get; set; }
        public string? Token { get; set; }
    }

    // =========================
    // Combined User + Profile
    // =========================
    public class MobileUserDto
    {
        public Guid Id { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }

        public string? Role { get; set; }      // First identity role name (if any)
        public string? AppRole { get; set; }   // If you store it in ApplicationUser

        // Profile
        public string? FullName { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? Language { get; set; }
        public string? AvatarUrl { get; set; }

        public bool IsVerified { get; set; } // if you need it from InfluencerProfile later
        public DateTime? CreateDate { get; set; } // if available on your user base class
    }

    // =========================
    // Requests
    // =========================
    public class RegisterRequestDto
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? PhoneNumber { get; set; }

        // Optional role to assign
        public string? Role { get; set; } = "Client";

        // Profile fields
        public string? FullName { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? Language { get; set; }
        public string? AvatarUrl { get; set; }
    }

    public class LoginRequestDto
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }

    public class UpdateAccountRequestDto
    {
        public string? PhoneNumber { get; set; }
        public string? FullName { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? Language { get; set; }
        public string? AvatarUrl { get; set; }
    }

    public class ChangePasswordRequestDto
    {
        public string CurrentPassword { get; set; } = null!;
        public string NewPassword { get; set; } = null!;
    }

    public class ForgetPasswordRequestDto
    {
        public string Email { get; set; } = null!;
    }

    public class ResetPasswordRequestDto
    {
        public string Email { get; set; } = null!;
        public string Token { get; set; } = null!;
        public string NewPassword { get; set; } = null!;
    }
}

