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
        Task<IEnumerable<KeyWordsDTo>> GetAllKeyWords();
        Task<KeyWordsDTo?> GetMetaPageById(int id);
        Task<bool> AddNewKeyWords(KeyWordsDTo createkeyWordsDTo);
        Task<bool> UpdateKeyWords(KeyWordsDTo updatekeyWordsDTo);
        Task<bool> DeleteKeyWordsAsync(int id);
    }
}
