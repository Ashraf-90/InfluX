using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class UserNicheDto : CommonDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int NicheId { get; set; }
    }

    public class UserNicheCreateDto : CommonCreateDto
    {
        public int UserId { get; set; }
        public int NicheId { get; set; }
    }

    public class UserNicheDeleteDto { public int Id { get; set; } }
}

