using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Forum.DAL.Models;

namespace Forum.BLL.Services.Communication
{
    public class UserResponse : BaseResponse<User>
    {
        public UserResponse(User user) : base(user) { }
        public UserResponse(string message) : base(message) { }
    }
}
