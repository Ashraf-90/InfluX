using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class KeyWordsDTo
    {
        public int Id { get; set; }
        public string? EnKeyword { get; set; }
        public string? ArKeyword { get; set; }
    }
}
