using Forum.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Repositories
{
    public class CalendarRepository : Repository<Calendar>, ICalendarRepository
    {
        public CalendarRepository(ForumAppDbContext context) : base(context) { }
        public async Task<Calendar> FindInstance(string PostId, string UserId)
        {
            return await (from calendar in _context.Calendar
                          where calendar.EventId == PostId
                          where calendar.UserId == UserId
                          select calendar).FirstOrDefaultAsync();
        }
    }
}
