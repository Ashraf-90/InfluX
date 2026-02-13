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

        public async Task<IEnumerable<KeyWordsDto>> GetAllKeyWords()
        {
            var data = await unitOfWork.KeyWords.GetAllAsync();
            return mapper.Map<IEnumerable<KeyWordsDto>>(data);
        }

        public async Task<KeyWordsDto?> GetById(Guid id)
        {
            var list = await unitOfWork.KeyWords.GetAllAsyncWitFillter(
                new List<Expression<Func<KeyWords, bool>>> { x => x.Id == id });

            var entity = list.FirstOrDefault();
            return entity == null ? null : mapper.Map<KeyWordsDto>(entity);
        }

        public async Task<bool> AddNewKeyWords(KeyWordsCreateDto dto)
        {
            var entity = mapper.Map<KeyWords>(dto);
            var ok = await unitOfWork.KeyWords.AddAsync(entity);
            await unitOfWork.SaveChangesAsync();
            return ok;
        }

        public async Task<bool> UpdateKeyWords(KeyWordsUpdateDto dto)
        {
            var list = await unitOfWork.KeyWords.GetAllAsyncWitFillter(
                new List<Expression<Func<KeyWords, bool>>> { x => x.Id == dto.Id });

            var entity = list.FirstOrDefault();
            if (entity == null) return false;

            mapper.Map(dto, entity);
            var ok = await unitOfWork.KeyWords.UpdateAsync(entity);
            await unitOfWork.SaveChangesAsync();
            return ok;
        }

        // SoftDelete
        public async Task<bool> DeleteKeyWordsAsync(Guid id)
        {
            var list = await unitOfWork.KeyWords.GetAllAsyncWitFillter(
                new List<Expression<Func<KeyWords, bool>>> { x => x.Id == id });

            var entity = list.FirstOrDefault();
            if (entity == null) return false;

            entity.Active = false;
            var ok = await unitOfWork.KeyWords.UpdateAsync(entity);
            await unitOfWork.SaveChangesAsync();
            return ok;
        }
    }
}

