using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IPixelsServices
    {
        Task<IEnumerable<PixelsDTo>> GetAllPixels();
        Task<PixelsDTo?> GetMetaPageById(int id);
        Task<bool> AddNewPixels(PixelsDTo createPixelsDTo);
        Task<bool> UpdatePixels(PixelsDTo updatePixelsDTo);
        Task<bool> DeletePixelsAsync(int id);
    }
}
