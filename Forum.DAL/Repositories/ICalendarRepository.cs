using Forum.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.DAL.Repositories
{
    public interface ICalendarRepository : IRepository<Calendar>
    {
        Task<Calendar> FindInstance(string PostId, string UserId);
    }
}
