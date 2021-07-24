using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Forum.DAL.Models;

namespace Forum.DAL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ForumAppDbContext _context;
        public UnitOfWork(ForumAppDbContext context)
        {
            _context = context;
        }
        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
