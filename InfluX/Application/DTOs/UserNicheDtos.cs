using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class UserNicheDto : CommonDto
    {
        public Guid UserId { get; set; }
        public Guid NicheId { get; set; }
    }

    public class UserNicheCreateDto : CommonCreateDto
    {
        public Guid UserId { get; set; }
        public Guid NicheId { get; set; }
    }

    // Pivot عادة لا يحتاج Update، لكن إذا أردت:
    public class UserNicheUpdateDto : CommonCreateDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid NicheId { get; set; }
    }

    public class UserNicheDeleteDto
    {
        public Guid Id { get; set; }
    }
}


