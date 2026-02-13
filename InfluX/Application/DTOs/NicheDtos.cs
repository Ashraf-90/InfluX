using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class NicheDto : CommonDto
    {
        public int Id { get; set; }
        public string NameAr { get; set; } = null!;
        public string NameEn { get; set; } = null!;
        public string? Icon { get; set; }
    }

    public class NicheCreateDto : CommonCreateDto
    {
        public string NameAr { get; set; } = null!;
        public string NameEn { get; set; } = null!;
        public string? Icon { get; set; }
    }

    public class NicheUpdateDto : CommonCreateDto
    {
        public int Id { get; set; }
        public string NameAr { get; set; } = null!;
        public string NameEn { get; set; } = null!;
        public string? Icon { get; set; }
    }

    public class NicheDeleteDto { public int Id { get; set; } }
}

