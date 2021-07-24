using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Forum.BLL.Services.Communication;
using Forum.DAL.Models;

namespace Forum.BLL.Services
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
        Task<bool> UserOwnsComentAsync(string ComentId, string UserId);

    }
}
