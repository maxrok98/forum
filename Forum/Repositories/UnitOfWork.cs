using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Forum.Models;

namespace Forum.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Models.AppContext _context;
        public UnitOfWork(Models.AppContext context)
        {
            _context = context;
        }
        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
