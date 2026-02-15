using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs
{
    public class UserNichesBulkCreateDto
    {
        [Required]
        [MinLength(1, ErrorMessage = "At least one nicheId is required.")]
        public List<Guid> NicheIds { get; set; } = new();
    }
}

