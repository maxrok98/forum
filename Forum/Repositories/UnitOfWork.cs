using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Forum.Models;

namespace Forum.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Models.ForumAppDbContext _context;
        public UnitOfWork(Models.ForumAppDbContext context)
        {
            _context = context;
        }
        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
