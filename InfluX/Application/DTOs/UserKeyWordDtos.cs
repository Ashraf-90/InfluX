using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class UserKeyWordDto : CommonDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int KeyWordsId { get; set; }
    }

    public class UserKeyWordCreateDto : CommonCreateDto
    {
        public int UserId { get; set; }
        public int KeyWordsId { get; set; }
    }

    public class UserKeyWordDeleteDto { public int Id { get; set; } }
}

