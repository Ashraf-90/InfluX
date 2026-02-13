using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Abstractions
{
    public interface IUnitOfWork : IDisposable 
    {
        
        IRepository<MetaPages> MetaPages { get; }
        IRepository<KeyWords> KeyWords { get; }
        IRepository<Pixels> Pixels { get; }




        Task<int> CompleteAsync();
        Task<int> SaveChangesAsync();
    }
}
