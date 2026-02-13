using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class UserProfileDto : CommonDto
    {
        public Guid UserId { get; set; }

        public string? FullName { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? Language { get; set; }
        public string? AvatarUrl { get; set; }
    }

    public class UserProfileCreateDto : CommonCreateDto
    {
        public Guid UserId { get; set; }

        public string? FullName { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? Language { get; set; }
        public string? AvatarUrl { get; set; }
    }

    public class UserProfileUpdateDto : CommonCreateDto
    {
        public Guid Id { get; set; }

        public string? FullName { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? Language { get; set; }
        public string? AvatarUrl { get; set; }
    }

    public class UserProfileDeleteDto
    {
        public Guid Id { get; set; }
    }
}


