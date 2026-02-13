using AutoMapper;
using Domain.Abstractions;
using Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace Application.Interfaces
{
    public abstract class BaseCrudServices<TEntity, TDto, TCreateDto, TUpdateDto>
        : ICrudServices<TDto, TCreateDto, TUpdateDto>
        where TEntity : Common
    {
        protected readonly IUnitOfWork unitOfWork;
        protected readonly IMapper mapper;
        protected readonly IRepository<TEntity> repo;

        protected BaseCrudServices(IUnitOfWork unitOfWork, IMapper mapper, IRepository<TEntity> repo)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.repo = repo;
        }

        public async Task<IEnumerable<TDto>> GetAll()
        {
            var data = await repo.GetAllAsync();
            return mapper.Map<IEnumerable<TDto>>(data);
        }

        public async Task<TDto?> GetById(Guid id)
        {
            var list = await repo.GetAllAsyncWitFillter(
                new List<Expression<Func<TEntity, bool>>> { x => x.Id == id });

            var entity = list.FirstOrDefault();
            return entity == null ? default : mapper.Map<TDto>(entity);
        }

        public async Task<bool> Create(TCreateDto dto)
        {
            var entity = mapper.Map<TEntity>(dto);
            var ok = await repo.AddAsync(entity);
            await unitOfWork.SaveChangesAsync();
            return ok;
        }

        public async Task<bool> Update(TUpdateDto dto)
        {
            var entityId = GetUpdateId(dto);

            var list = await repo.GetAllAsyncWitFillter(
                new List<Expression<Func<TEntity, bool>>> { x => x.Id == entityId });

            var entity = list.FirstOrDefault();
            if (entity == null) return false;

            mapper.Map(dto, entity);
            var ok = await repo.UpdateAsync(entity);
            await unitOfWork.SaveChangesAsync();
            return ok;
        }

        public async Task<bool> SoftDelete(Guid id)
        {
            var list = await repo.GetAllAsyncWitFillter(
                new List<Expression<Func<TEntity, bool>>> { x => x.Id == id });

            var entity = list.FirstOrDefault();
            if (entity == null) return false;

            entity.Active = false;
            var ok = await repo.UpdateAsync(entity);
            await unitOfWork.SaveChangesAsync();
            return ok;
        }

        protected abstract Guid GetUpdateId(TUpdateDto dto);
    }
}

