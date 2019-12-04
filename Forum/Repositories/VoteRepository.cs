using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Forum.Models;
using Microsoft.EntityFrameworkCore;

namespace Forum.Repositories
{
    public class VoteRepository : Repository<Vote>, IVoteRepository
    {
        public VoteRepository(Models.AppContext context) : base(context) { }

        public async Task<Vote> FindInstance(string PostId, string UserId)
        {
            return await (from vote in _context.Votes
                          where vote.PostId == PostId
                          where vote.UserId == UserId
                          select vote).FirstAsync();
        }
    }
}
