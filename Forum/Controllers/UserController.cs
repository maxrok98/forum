using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Forum.Extensions;
using Forum.Helpers;
using Forum.DAL.Models;
using Forum.BLL.Services;
using Forum.Shared.Contracts.Requests.Queries;
using Forum.Shared.Contracts.Requests;
using Forum.Shared.Contracts.Responses;
using Forum.Shared.Contracts;

namespace Forum.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IUriService _uriService;
        private readonly IMapper _mapper;
        public UserController(IUserService userService, IUriService uriService, IMapper mapper)
        {
            _userService = userService;
            _uriService = uriService;
            _mapper = mapper;
        }

        [HttpGet("get", Name = "GetUsers")]
        [AllowAnonymous]
        public async Task<IActionResult> Get([FromQuery]string userName, [FromQuery]PaginationQuery paginationQuery)
        {
            var pagination = _mapper.Map<PaginationFilter>(paginationQuery);
            var users = await _userService.GetAllAsync(userName, pagination);
            var dto = _mapper.Map<IEnumerable<User>, IEnumerable<UserShortResponse>>(users.Resource);

            if(pagination == null || pagination.PageNumber < 1 || pagination.PageSize < 1)
            {
                return Ok(dto);
            }

            var paginationResponse = PaginationHelpers.CreatePaginatedResponse(_uriService, pagination, dto, users.amountOfUsers, ApiRoutes.User.GetAll);

            return Ok(paginationResponse);

        }

        [HttpGet("get/{id}", Name = "GetUser")]
        [AllowAnonymous]
        public async Task<IActionResult> Get(string id)
        {
            var user = await _userService.GetAsync(id);
            var dto = _mapper.Map<User, UserResponse>(user);

            return Ok(dto);
        }

        [HttpGet("get-short/{id}", Name = "GetShortUser")]
        [AllowAnonymous]
        public async Task<IActionResult> GetShort(string id)
        {
            var user = await _userService.GetAsync(id);
            var dto = _mapper.Map<User, UserShortResponse>(user);

            return Ok(dto);
        }

        [HttpPut("Update-Password/{id}", Name = "UpdatePassword")]
        public async Task<IActionResult> UpdatePassword(string id, UpdatePasswordRequest password)
        {
            var userId = HttpContext.GetUserId();
            if(id != userId)
            {
                return BadRequest("This account is not your!");
            }
            var result = await _userService.UpdatePasswordAsync(userId, password.currentPassword, password.newPassword);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok("Password succesfuly changed!");
        }

        [HttpPut("Update-Image/{id}", Name = "UpdateImage")]
        public async Task<IActionResult> UpdateImage(string id, UpdateImageRequest image)
        {
            var userId = HttpContext.GetUserId();
            if(id != userId)
            {
                return BadRequest("This account is not your!");
            }

            var result = await _userService.UpdateImageAsync(userId, image.Image);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok(result.Resource.ImageLink);
        }

        [HttpPut("Update-Account/{id}", Name = "UpdateAccount")]
        public async Task<IActionResult> UpdateAccount(string id, UpdateAccountRequest info)
        {
            var userId = HttpContext.GetUserId();
            if(id != userId)
            {
                return BadRequest("This account is not your!");
            }

            var result = await _userService.UpdateAccountAsync(userId, info.Email, info.UserName);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            var dto = _mapper.Map<User, UserShortResponse>(result.Resource);

            return Ok(dto);
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
                return BadRequest(result.Message);
            }

            return Ok();
        }

    }
}