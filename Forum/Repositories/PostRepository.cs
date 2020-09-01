using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Forum.Models;
using Forum.Repositories.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace Forum.Repositories
{
    public class PostRepository : Repository<Post>, IPostRepository
    {
        public PostSorting sorting;
        public PostRepository(Models.ForumAppDbContext context) : base(context) {
            sorting = new PostSorting();
        }

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

        public async Task<int> GetCountOfFilteredPostsInThreadAsync(string postName, string threadId)
        {
            IQueryable<Post> query = _context.Posts;
            if (!string.IsNullOrEmpty(postName))
                query = query.Where(p => p.Name.Contains(postName));
            if (!string.IsNullOrEmpty(threadId))
                query = query.Where(p => p.ThreadId == threadId);
            return query.Count();
        }

        public override async Task<IEnumerable<Post>> GetAllAsync()
        {
            return await _context.Posts.Include(p => p.Coments).ThenInclude(p => p.SubComents).Include(p => p.Thread).Include(p => p.User).Include(p => p.Image).ToListAsync();
        }
        public async Task<IEnumerable<Post>> GetFilteredAndPagedFromThreadAsync(string postName, string threadId, PaginationFilter paginationFilter, string orderByQueryString)
        {
            IQueryable<Post> query = _context.Posts.Include(p => p.Coments).ThenInclude(p => p.SubComents).Include(p => p.Thread).Include(p => p.User).Include(p => p.Image);
            if (!string.IsNullOrEmpty(postName))
                query = query.Where(p => p.Name.Contains(postName));
            if (!string.IsNullOrEmpty(threadId))
                query = query.Where(p => p.ThreadId == threadId);
            query = query.OrderBy(sorting.ApplySort(orderByQueryString));
            if (paginationFilter != null)
            {
                var skip = (paginationFilter.PageNumber - 1) * paginationFilter.PageSize;
                query = query.Skip(skip).Take(paginationFilter.PageSize);
            }
            return await query.AsNoTracking().ToListAsync();

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
