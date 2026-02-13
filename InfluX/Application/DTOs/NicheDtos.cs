using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class NicheDto : CommonDto
    {
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
        public Guid Id { get; set; }

        public string NameAr { get; set; } = null!;
        public string NameEn { get; set; } = null!;
        public string? Icon { get; set; }
    }

    public class NicheDeleteDto
    {
        public Guid Id { get; set; }
    }
}


