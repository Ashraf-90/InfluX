using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class PixelsDTo
    {
        public int Id { get; set; }
        public string? PixelsApp { get; set; }
        public string? PixelsCode { get; set; }
    }
}
