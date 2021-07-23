using Forum.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Repositories
{
    public class ChatRepository : Repository<Chat>, IChatRepository
    {
        public ChatRepository(Models.ForumAppDbContext context) : base(context) { }

        public override async Task<Chat> GetAsync(string id)
        {
            var res =  _context.Chats.Where(c => c.Id == id).Include(c => c.Users).ThenInclude(u => u.User).Include(c => c.Messages)
            .Select(u => new Chat() {
                Id = u.Id,
                Name = u.Name,
                Users = u.Users,
                Messages = u.Messages.OrderBy(m => m.Date).ToList()
            }).AsEnumerable().FirstOrDefault();
            return res;
        } 

        public override async Task<IEnumerable<Chat>> GetAllAsync()
        {
            return await _context.Chats.Include(c => c.Users).ThenInclude(c => c.User).AsQueryable().ToListAsync();
        }
        public async Task<IEnumerable<Chat>> GetAllFromUserAsync(string id)
        {
            var res =  _context.Chats.Where(x => x.Users.Any(u => u.UserId == id)).Include(c => c.Users).ThenInclude(c => c.User)
            .Select(u => new Chat()
            {
                Id = u.Id,
                Name = u.Name,
                Users = u.Users,
                Messages = new List<Message>() { u.Messages.OrderByDescending(m => m.Date).FirstOrDefault() }

            })
            .AsEnumerable();
            return res;
        }
    }
}
