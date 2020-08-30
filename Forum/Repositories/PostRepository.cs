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
        public PostRepository(Models.ForumAppDbContext context) : base(context) { }

        public async Task<IEnumerable<Post>> GetOrderByVoteAsync()
        {
            var query = from vote in _context.Votes
                        join post in _context.Posts on vote.PostId equals post.Id
                        group vote by post.Id into grouped
                        select new { postId = grouped.Key, Count = grouped.Count(t => t.Id != null) };

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

        public async Task<int> GetCountOfAllPostsAsync()
        {
            return await _context.Posts.CountAsync();
        }
        public async Task<int> GetCountOfFilteredPostsAsync(string postName)
        {
            return await _context.Posts.Where(p => p.Name.Contains(postName)).CountAsync();
        }
        public async Task<int> GetCountOfPostsInThreadAsync(string threadId)
        {
            return await _context.Posts.Where(p => p.ThreadId == threadId).CountAsync();
        }
        public async Task<int> GetCountOfFilteredPostsInThreadAsync(string postName, string threadId)
        {
            return await _context.Posts.Where(p => p.Name.Contains(postName)).Where(p => p.ThreadId == threadId).CountAsync();
        }

        public override async Task<IEnumerable<Post>> GetAllAsync()
        {
            return await _context.Posts.Include(p => p.Coments).ThenInclude(p => p.SubComents).Include(p => p.Thread).Include(p => p.User).Include(p => p.Image).ToListAsync();
        }
        public async Task<IEnumerable<Post>> GetPaged(PaginationFilter paginationFilter, int skip)
        {
            return await _context.Posts.Include(p => p.Coments).ThenInclude(p => p.SubComents).Include(p => p.Thread).Include(p => p.User).Include(p => p.Image).Skip(skip).Take(paginationFilter.PageSize).ToListAsync();
        }
        public async Task<IEnumerable<Post>> GetFilteredAsync(string postName)
        {
            return await _context.Posts.Where(p => p.Name.Contains(postName)).Include(p => p.Coments).ThenInclude(p => p.SubComents).Include(p => p.Thread).Include(p => p.User).Include(p => p.Image).ToListAsync();
        }
        public async Task<IEnumerable<Post>> GetFromThreadAsync(string threadId)
        {
            return await _context.Posts.Where(p => p.ThreadId == threadId).Include(p => p.Coments).ThenInclude(p => p.SubComents).Include(p => p.Thread).Include(p => p.User).Include(p => p.Image).ToListAsync();
        }
        public async Task<IEnumerable<Post>> GetPagedFromThreadAsync(string threadId, PaginationFilter paginationFilter, int skip)
        {
            return await _context.Posts.Where(p => p.ThreadId == threadId).Include(p => p.Coments).ThenInclude(p => p.SubComents).Include(p => p.Thread).Include(p => p.User).Include(p => p.Image).Skip(skip).Take(paginationFilter.PageSize).ToListAsync();
        }
        public async Task<IEnumerable<Post>> GetFilteredAndPagedAsync(string postName, PaginationFilter paginationFilter, int skip)
        {
            return await _context.Posts.Where(p => p.Name.Contains(postName)).Include(p => p.Coments).ThenInclude(p => p.SubComents).Include(p => p.Thread).Include(p => p.User).Include(p => p.Image).Skip(skip).Take(paginationFilter.PageSize).ToListAsync();
        }
        public async Task<IEnumerable<Post>> GetFilteredFromThreadAsync(string postName, string threadId)
        {
            return await _context.Posts.Where(p => p.Name.Contains(postName)).Where(p => p.ThreadId == threadId).Include(p => p.Coments).ThenInclude(p => p.SubComents).Include(p => p.Thread).Include(p => p.User).Include(p => p.Image).ToListAsync();
        }
        public async Task<IEnumerable<Post>> GetFilteredAndPagedFromThreadAsync(string postName, string threadId, PaginationFilter paginationFilter, int skip)
        {
            return await _context.Posts.Where(p => p.Name.Contains(postName)).Where(p => p.ThreadId == threadId).Include(p => p.Coments).ThenInclude(p => p.SubComents).Include(p => p.Thread).Include(p => p.User).Include(p => p.Image).Skip(skip).Take(paginationFilter.PageSize).ToListAsync();
        }


        public override async Task<Post> GetAsync(string id)
        {
            return await (from p in _context.Posts
                          where p.Id == id
                          select p).Include(p => p.Thread).Include(p => p.User).Include(p => p.Image).Include(p => p.Coments).FirstOrDefaultAsync();
        }
        public async Task<Post> UserOwnsPostAsync(string PostId, string UserId)
        {
            return await (from p in _context.Posts
                          where p.Id == PostId
                          where p.UserId == UserId
                          select p).FirstAsync();
        }
    }
}
