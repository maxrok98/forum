using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Forum.Models;

namespace Forum.Repositories
{
    public interface IComentRepository : IRepository<Coment>
    {
        Task<IEnumerable<Coment>> GetAllFromUserAsync(string id);
        Task<IEnumerable<Coment>> GetAllFromPostAsync(string id);
        Task<Coment> UserOwnsComentAsync(string ComentId, string UserId);
    }
}
