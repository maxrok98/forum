using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Forum.Services;
using AutoMapper;
using Forum.DTOout;
using Forum.DTOin;
using Forum.Models;
using Forum.Extensions;

namespace Forum.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        public UserController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet("get", Name = "GetUsers")]
        [ProducesResponseType(typeof(IEnumerable<UserDTOout>), 200)]
        public async Task<IEnumerable<UserDTOout>> Get()
        {
            var users = await _userService.GetAllAsync();
            var dto = _mapper.Map<IEnumerable<User>, IEnumerable<UserDTOout>>(users);

            return dto;
        }

        [HttpGet("get/{id}", Name = "GetUser")]
        [ProducesResponseType(typeof(IEnumerable<UserDTOout>), 200)]
        public async Task<UserDTOout> Get(string id)
        {
            var user = await _userService.GetAsync(id);
            var dto = _mapper.Map<User, UserDTOout>(user);

            return dto;
        }

        [HttpPut("ChangePassword/{id}", Name = "ChangePassword")]
        public async Task<IActionResult> ChangePassword(string id, ChangePasswordDTOin password)
        {
            var userId = HttpContext.GetUserId();
            if(id != userId)
            {
                return BadRequest("This account is not your!");
            }
            var result = await _userService.UpdatePasswordAsync(userId, password.currentPassword, password.newPassword);

            if (!result.Success)
            {
                return BadRequest(new ErrorViewModel());
            }

            return Ok();
        }

        [HttpDelete("delete/{id}", Name = "DeleteUser")]
        public async Task<IActionResult> Delete(string id)
        {
            var userId = HttpContext.GetUserId();
            var admin = _userService.IsUserAdmin(userId).Result;
            if(id != userId && !admin)
            {
                return BadRequest("This account is not your!");
            }

            var result = await _userService.RemoveAsync(id);

            if (!result.Success)
            {
                return BadRequest(new ErrorViewModel());
            }

            return Ok();
        }

    }
}