using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Forum.Models;

namespace Forum.Repositories
{
    public interface IVoteRepository : IRepository<Vote>
    {
        Task<Vote> FindInstance(string PostId, string UserId);
    }
}
