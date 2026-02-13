using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ICrudServices<TDto, TCreateDto, TUpdateDto>
    {
        Task<IEnumerable<TDto>> GetAll();
        Task<TDto?> GetById(Guid id);
        Task<bool> Create(TCreateDto dto);
        Task<bool> Update(TUpdateDto dto);
        Task<bool> SoftDelete(Guid id);
    }
}

