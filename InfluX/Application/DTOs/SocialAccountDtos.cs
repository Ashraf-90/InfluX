using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class SocialAccountDto : CommonDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public SocialPlatform Platform { get; set; }
        public string? Handle { get; set; }
        public string? ProfileUrl { get; set; }
        public int Followers { get; set; }
        public int AvgViews { get; set; }
        public decimal EngagementRate { get; set; }
        public bool IsConnected { get; set; }
    }

    public class SocialAccountCreateDto : CommonCreateDto
    {
        public int UserId { get; set; }
        public SocialPlatform Platform { get; set; }
        public string? Handle { get; set; }
        public string? ProfileUrl { get; set; }
        public int Followers { get; set; }
        public int AvgViews { get; set; }
        public decimal EngagementRate { get; set; }
        public bool IsConnected { get; set; } = false;
    }

    public class SocialAccountUpdateDto : CommonCreateDto
    {
        public int Id { get; set; }
        public string? Handle { get; set; }
        public string? ProfileUrl { get; set; }
        public int Followers { get; set; }
        public int AvgViews { get; set; }
        public decimal EngagementRate { get; set; }
        public bool IsConnected { get; set; }
    }

    public class SocialAccountDeleteDto { public int Id { get; set; } }
}

