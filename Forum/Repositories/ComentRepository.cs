using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Forum.Models;
using Microsoft.EntityFrameworkCore;

namespace Forum.Repositories
{
    public class ComentRepository : Repository<Coment>, IComentRepository
    {
        public ComentRepository(Models.ForumAppDbContext context) : base(context) { }

        public async Task<IEnumerable<Coment>> GetAllFromPostAsync(string id)
        {
            return await (from c in _context.Coments
                         where c.PostId == id
                         select c).AsQueryable().ToListAsync();
        }

        public async Task<IEnumerable<Coment>> GetAllFromUserAsync(string id)
        {
            return await (from c in _context.Coments
                          where c.UserId == id
                          select c).AsQueryable().ToListAsync();
        }

        public override async Task<IEnumerable<Coment>> GetAllAsync()
        {
            return await _context.Coments.Include(c => c.User).ToListAsync();
        }

        public override async Task<Coment> GetAsync(string id)
        {
            return await (from c in _context.Coments
                          where c.Id == id
                          select c).Include(c => c.User).FirstAsync();
        }
        public async Task<Coment> UserOwnsComentAsync(string ComentId, string UserId)
        {
            return await (from c in _context.Coments
                          where c.Id == ComentId
                          where c.UserId == UserId
                          select c).FirstAsync();
        }
    }
}
