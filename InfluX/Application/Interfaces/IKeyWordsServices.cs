using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IKeyWordsServices
    {
        Task<IEnumerable<KeyWordsDto>> GetAllKeyWords();
        Task<KeyWordsDto?> GetById(Guid id);

        Task<bool> AddNewKeyWords(KeyWordsCreateDto dto);
        Task<bool> UpdateKeyWords(KeyWordsUpdateDto dto);

        // SoftDelete => Active = false
        Task<bool> DeleteKeyWordsAsync(Guid id);
    }
}

