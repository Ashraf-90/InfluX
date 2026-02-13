using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IMetaPagesServices
    {
        Task<IEnumerable<MetaPagesDTo>> GetAllMetaPages();
        Task<MetaPagesDTo?> GetMetaPageById(int id);
        Task<bool> AddNewMetaPages(MetaPagesDTo createMetaPagesDTo);
        Task<bool> UpdateMetaPages(MetaPagesDTo createMetaPagesDTo);
    }
}
