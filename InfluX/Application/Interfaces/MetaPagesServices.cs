using Application.DTOs;
using AutoMapper;
using Domain.Abstractions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public class MetaPagesServices : IMetaPagesServices
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        public MetaPagesServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        

        public async Task<bool> AddNewMetaPages(MetaPagesDTo createMetaPagesDTo)
        {
            var data = mapper.Map<MetaPages>(createMetaPagesDTo);
            var result = await unitOfWork.MetaPages.AddAsync(data);
            await unitOfWork.SaveChangesAsync();
            return result;
        }

        public async Task<bool> UpdateMetaPages(MetaPagesDTo updateMetaPagesDTo)
        {
            var MetaPages = await unitOfWork.MetaPages.GetAllAsyncWitFillter(
                new List<Expression<Func<MetaPages, bool>>> { x => x.Id == updateMetaPagesDTo.Id });

            var entity = MetaPages.FirstOrDefault();
            if (entity == null) return false;

            mapper.Map(updateMetaPagesDTo, entity); // AutoMapper updates fields
            var result = await unitOfWork.MetaPages.UpdateAsync(entity);
            await unitOfWork.SaveChangesAsync();
            return result;
        }


        public async Task<IEnumerable<MetaPagesDTo>> GetAllMetaPages()
        {
            var data = await unitOfWork.MetaPages.GetAllAsync();
            var result = mapper.Map<IEnumerable<MetaPagesDTo>>(data);

            return result;
        }


        public async Task<MetaPagesDTo?> GetMetaPageById(int id)
        {
            var metaPages = await unitOfWork.MetaPages.GetAllFillterIncludeData(filters: new List<Expression<Func<MetaPages, bool>>> { x => x.Id == id } );
            var entity = metaPages.FirstOrDefault();
            return entity == null ? null : mapper.Map<MetaPagesDTo>(entity);
        }
    }
}
