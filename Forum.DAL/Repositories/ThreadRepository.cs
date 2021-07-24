using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Forum.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace Forum.DAL.Repositories
{
    public class ThreadRepository : Repository<Thread>, IThreadRepository
    {
        private readonly DbSet<Thread> _entity;
        public ThreadRepository(ForumAppDbContext context) : base(context)
        {
            _entity = context.Threads;
        }

        public override async Task<IEnumerable<Thread>> GetAllAsync()
        {
            return await _entity.Include(x => x.Posts).Include(t => t.Subscriptions).ToListAsync();
        }

        public override async Task<Thread> GetAsync(string id)
        {
            //return await _entity.Include(thread => thread.Image).Where(Id == id);
            return await (from t in _entity
                          where t.Id == id
                          select t).Include(x => x.Posts).Include(t => t.Subscriptions).FirstAsync();
        }
    }
}
