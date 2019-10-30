using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Forum.Models;
using Forum.Services.Communication;

namespace Forum.Services
{
    public interface IComentService
    {
        Task<IEnumerable<Coment>> GetAllAsync();
        Task<Coment> GetAsync(string id);
        Task<IEnumerable<Coment>> GetAllFromPostAsync(string id);
        Task<IEnumerable<Coment>> GetAllFromUserAsync(string id);
        Task<ComentResponse> AddAsync(Coment coment);
        Task<ComentResponse> UpdateAsync(string id, Coment coment);
        Task<ComentResponse> RemoveAsync(string id);

    }
}
