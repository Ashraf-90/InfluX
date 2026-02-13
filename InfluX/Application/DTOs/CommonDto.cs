using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class CommonDto
    {
        public bool Active { get; set; }
        public bool IsAvilable { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }

    public class CommonCreateDto
    {
        public bool Active { get; set; } = true;
        public bool IsAvilable { get; set; } = true;
    }
}

