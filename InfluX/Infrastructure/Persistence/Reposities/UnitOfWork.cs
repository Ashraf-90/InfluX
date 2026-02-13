using Domain.Abstractions;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Reposities
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDBContext _context;

        //===================  SEO ===================
        private IRepository<MetaPages> _MetaPages;
        private IRepository<KeyWords> _KeyWords;
        private IRepository<Pixels> _Pixels;

        public UnitOfWork(AppDBContext context)
        {
            _context = context;
        }








        //********************************* Get list **************************************************************

        public IRepository<MetaPages> MetaPages => _MetaPages ??= new Repository<MetaPages>(_context);
        public IRepository<KeyWords> KeyWords => _KeyWords ??= new Repository<KeyWords>(_context);
        public IRepository<Pixels> Pixels => _Pixels ??= new Repository<Pixels>(_context);


        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        
    }
}
