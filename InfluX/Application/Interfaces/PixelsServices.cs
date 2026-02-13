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
    public class PixelsServices : IPixelsServices
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        public PixelsServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<bool> AddNewPixels(PixelsDTo createPixelsDTo)
        {
            var data = mapper.Map<Pixels>(createPixelsDTo);
            var result = await unitOfWork.Pixels.AddAsync(data);
            await unitOfWork.SaveChangesAsync();
            return result;
        }

        public async Task<bool> UpdatePixels(PixelsDTo updatePixelsDTo)
        {
            var Pixels = await unitOfWork.Pixels.GetAllAsyncWitFillter(
                new List<Expression<Func<Pixels, bool>>> { x => x.Id == updatePixelsDTo.Id });

            var entity = Pixels.FirstOrDefault();
            if (entity == null) return false;

            mapper.Map(updatePixelsDTo, entity); // AutoMapper updates fields
            var result = await unitOfWork.Pixels.UpdateAsync(entity);
            await unitOfWork.SaveChangesAsync();
            return result;
        }

        public async Task<IEnumerable<PixelsDTo>> GetAllPixels()
        {
            var data = await unitOfWork.Pixels.GetAllAsync();
            var result = mapper.Map<IEnumerable<PixelsDTo>>(data);

            return result;
        }

        public async Task<PixelsDTo?> GetMetaPageById(int id)
        {
            var Pixels = await unitOfWork.Pixels.GetAllFillterIncludeData(filters: new List<Expression<Func<Pixels, bool>>> { x => x.Id == id });
            var entity = Pixels.FirstOrDefault();
            return entity == null ? null : mapper.Map<PixelsDTo>(entity);
        }

        public async Task<bool> DeletePixelsAsync(int id)
        {
            var allPixels = await unitOfWork.Pixels.GetAllAsync();
            var DeletedKeyWord = allPixels.Where(b => b.Id == id);

            if (DeletedKeyWord == null || !DeletedKeyWord.Any())
                return false;

            unitOfWork.Pixels.DeleteRange(DeletedKeyWord);
            await unitOfWork.SaveChangesAsync();
            return true;
        }

        
    }
}
