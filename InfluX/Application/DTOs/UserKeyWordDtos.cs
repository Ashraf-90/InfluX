using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class UserKeyWordDto : CommonDto
    {
        public Guid UserId { get; set; }
        public Guid KeyWordsId { get; set; }
    }

    public class UserKeyWordCreateDto : CommonCreateDto
    {
        public Guid UserId { get; set; }
        public Guid KeyWordsId { get; set; }
    }

    // Pivot عادة لا يحتاج Update، لكن إذا أردت:
    public class UserKeyWordUpdateDto : CommonCreateDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid KeyWordsId { get; set; }
    }

    public class UserKeyWordDeleteDto
    {
        public Guid Id { get; set; }
    }
}


