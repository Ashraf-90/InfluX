using Application.DTOs;
using AutoMapper;
using Domain.Abstractions;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public class KeyWordsServices : IKeyWordsServices
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        public KeyWordsServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<bool> AddNewKeyWords(KeyWordsDTo createKeyWordsDTo)
        {
            var data = mapper.Map<KeyWords>(createKeyWordsDTo);
            var result = await unitOfWork.KeyWords.AddAsync(data);
            await unitOfWork.SaveChangesAsync();
            return result;
        }

        public async Task<bool> UpdateKeyWords(KeyWordsDTo updateKeyWordsDTo)
        {
            var KeyWords = await unitOfWork.KeyWords.GetAllAsyncWitFillter(
                new List<Expression<Func<KeyWords, bool>>> { x => x.Id == updateKeyWordsDTo.Id });

            var entity = KeyWords.FirstOrDefault();
            if (entity == null) return false;

            mapper.Map(updateKeyWordsDTo, entity); // AutoMapper updates fields
            var result = await unitOfWork.KeyWords.UpdateAsync(entity);
            await unitOfWork.SaveChangesAsync();
            return result;
        }

        public async Task<IEnumerable<KeyWordsDTo>> GetAllKeyWords()
        {
            var data = await unitOfWork.KeyWords.GetAllAsync();
            var result = mapper.Map<IEnumerable<KeyWordsDTo>>(data);

            return result;
        }

        public async Task<KeyWordsDTo?> GetMetaPageById(int id)
        {
            var KeyWords = await unitOfWork.KeyWords.GetAllFillterIncludeData(filters: new List<Expression<Func<KeyWords, bool>>> { x => x.Id == id });
            var entity = KeyWords.FirstOrDefault();
            return entity == null ? null : mapper.Map<KeyWordsDTo>(entity);
        }

        public async Task<bool> DeleteKeyWordsAsync(int id)
        {
            var allKeyWords = await unitOfWork.KeyWords.GetAllAsync();
            var DeletedKeyWord = allKeyWords.Where(b => b.Id == id);

            if (DeletedKeyWord == null || !DeletedKeyWord.Any())
                return false;

            unitOfWork.KeyWords.DeleteRange(DeletedKeyWord);
            await unitOfWork.SaveChangesAsync();
            return true;
        }

    }
}
