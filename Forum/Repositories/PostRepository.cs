using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Forum.Models;
using Microsoft.EntityFrameworkCore;

namespace Forum.Repositories
{
    public class PostRepository : Repository<Post>, IPostRepository
    {
        public PostRepository(Models.AppContext context) : base(context) { }

        public async Task<IEnumerable<Post>> GetOrderByVoteAsync()
        {
            var query = from vote in _context.Votes
                        join post in _context.Posts on vote.PostId equals post.Id
                        group vote by post.Id into grouped
                        select new { postId = grouped.Key, Count = grouped.Count(t => t.Id != null) };

            //var query2 = (from post in _context.Posts
            //              select post).ToList();

            //foreach (var q in query2)
            //{
            //    Console.WriteLine($"{q.Content} - {q.Id}");
            //}

            return await (from p in query
                    from post in _context.Posts
                    where post.Id == p.postId
                    orderby p.Count descending
                    select post).ToListAsync();
        }
        public async Task<IEnumerable<Post>> GetOrderByDateAsync()
        {
            return await (from p in _context.Posts
                    orderby p.Date descending
                    select p).ToListAsync();
        }

        public override async Task<IEnumerable<Post>> GetAllAsync()
        {
            return await _context.Posts.Include(p => p.Thread).Include(p => p.User).Include(p => p.Image).ToListAsync();
        }
        public override async Task<Post> GetAsync(string id)
        {
            return await (from p in _context.Posts
                          where p.Id == id
                          select p).Include(p => p.Thread).Include(p => p.User).Include(p => p.Image).FirstAsync();
        }
    }
}
