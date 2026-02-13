using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class KeyWordsDto : CommonDto
    {
        public string? EnKeyword { get; set; }
        public string? ArKeyword { get; set; }
    }

    public class KeyWordsCreateDto : CommonCreateDto
    {
        public string? EnKeyword { get; set; }
        public string? ArKeyword { get; set; }
    }

    public class KeyWordsUpdateDto : CommonCreateDto
    {
        public Guid Id { get; set; }

        public string? EnKeyword { get; set; }
        public string? ArKeyword { get; set; }
    }

    public class KeyWordsDeleteDto
    {
        public Guid Id { get; set; }
    }
}

