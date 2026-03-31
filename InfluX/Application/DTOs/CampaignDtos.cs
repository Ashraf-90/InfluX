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
        public Guid BrandId { get; set; }
        public Guid? AgencyId { get; set; }

        public string Title { get; set; } = null!;
        public string? Objective { get; set; }

        public decimal TotalBudget { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public CampaignStatus Status { get; set; }
    }

    public class CampaignCreateDto : CommonCreateDto
    {
        public Guid BrandId { get; set; }
        public Guid? AgencyId { get; set; }

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

        public Guid BrandId { get; set; }
        public Guid? AgencyId { get; set; }

        public string Title { get; set; } = null!;
        public string? Objective { get; set; }

        public decimal TotalBudget { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public CampaignStatus Status { get; set; }
    }
}
