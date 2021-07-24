using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Forum.DAL.Models;

namespace Forum.BLL.Services.Communication
{
    public class ComentResponse : BaseResponse<Coment>
    {
        public ComentResponse(Coment coment) : base(coment) { }
        public ComentResponse(string message) : base(message) { }
    }
}
