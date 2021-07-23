using Forum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Repositories
{
    public interface IMessageRepository : IRepository<Message>
    {
    }
}
