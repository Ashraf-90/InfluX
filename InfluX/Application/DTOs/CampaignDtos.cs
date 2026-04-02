using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Domain.Entities;

namespace Application.DTOs
{
    public class CampaignDto : CommonDto
    {
        public Guid BrandProfileId { get; set; }
        public Guid? AgencyProfileId { get; set; }

        public string Title { get; set; } = null!;
        public string? Objective { get; set; }
        public decimal TotalBudget { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public CampaignStatus Status { get; set; }

        // Optional display helpers
        public string? BrandName { get; set; }
        public string? AgencyName { get; set; }
        public string CreatedBy { get; set; } = null!;
    }

    public class CampaignCreateDto : CommonCreateDto
    {
        public Guid BrandProfileId { get; set; }
        public Guid? AgencyProfileId { get; set; }

        public string Title { get; set; } = null!;
        public string? Objective { get; set; }
        public decimal TotalBudget { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public CampaignStatus Status { get; set; } = CampaignStatus.Draft;
    }

    public class CampaignUpdateDto : CommonCreateDto
    {
        public Guid Id { get; set; }

        public Guid BrandProfileId { get; set; }
        public Guid? AgencyProfileId { get; set; }

        public string Title { get; set; } = null!;
        public string? Objective { get; set; }
        public decimal TotalBudget { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public CampaignStatus Status { get; set; }
    }
}
