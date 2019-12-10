using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Forum.Services;
using AutoMapper;
using Forum.DTOin;
using Forum.DTOout;
using Forum.Models;
using Forum.Extensions;
using Forum.Contracts.Responses;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Forum.Contracts.Requests.Queries;
using Forum.Contracts;
using Forum.Helpers;

namespace Forum.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly IUriService _uriService;
        private readonly IMapper _mapper;
        public PostController(IPostService postService, IUriService uriService, IMapper mapper)
        {
            _postService = postService;
            _uriService = uriService;
            _mapper = mapper;
        }
        // GET: api/Post
        [HttpGet("get", Name = "GetPosts")]
        public async Task<IActionResult> Get([FromQuery]string postName, [FromQuery]PaginationQuery paginationQuery)
        {
            var pagination = _mapper.Map<PaginationFilter>(paginationQuery);
            var post = await _postService.GetAllAsync(postName, pagination);
            var dto = _mapper.Map<IEnumerable<Post>, IEnumerable<PostDTOout>>(post);

            if(pagination == null || pagination.PageNumber < 1 || pagination.PageSize < 1)
            {
                return Ok(new PageResponse<PostDTOout>(dto));
            }


            var paginationResponse = PaginationHelpers.CreatePaginatedResponse(_uriService, pagination, dto, ApiRoutes.Posts.GetAll);

            return Ok(paginationResponse);
        }

        // GET: api/Post/5
        [HttpGet("get/{id}", Name = "GetPost")]
        public async Task<IActionResult> Get(string id)
        {
            var post = await _postService.GetAsync(id);
            var dto = _mapper.Map<Post, PostDTOout>(post);

            return Ok(new Response<PostDTOout>(dto));
        }

        // POST: api/Post
        [HttpPost("post", Name = "PostPost")]
        public async Task<IActionResult> Post([FromBody] PostDTOin post)
        {
            var pt = _mapper.Map<PostDTOin, Post>(post);
            pt.UserId = HttpContext.GetUserId();
            var result = await _postService.AddAsync(pt);

            if (!result.Success)
            {
                return BadRequest(new ErrorViewModel());
            }

            var ptDTO = _mapper.Map<Post, PostDTOout>(result.Resource);
            return Ok(new Response<PostDTOout>(ptDTO));
        }

        // PUT: api/Post/5
        [HttpPut("put/{id}", Name = "PutPost")]
        public async Task<IActionResult> Put(string id, [FromBody] PostDTOin post)
        {
            var userOwnsPost = await _postService.UserOwnsPostAsync(id, HttpContext.GetUserId());

            if (!userOwnsPost)
            {
                return BadRequest(new ErrorResponse(new ErrorModel { Message = "You do not own this post" }));
            }

            var pt = _mapper.Map<PostDTOin, Post>(post);
            var result = await _postService.UpdateAsync(id, pt);

            if (!result.Success)
            {
                return BadRequest(new ErrorViewModel());
            }

            var ptDTO = _mapper.Map<Post, PostDTOout>(result.Resource);
            return Ok(new Response<PostDTOout>(ptDTO));
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("delete/{id}", Name = "DeletePost")]
        public async Task<IActionResult> Delete(string id)
        {
            var userOwnsPost = await _postService.UserOwnsPostAsync(id, HttpContext.GetUserId());

            if (!userOwnsPost)
            {
                return BadRequest(new ErrorResponse(new ErrorModel { Message = "You do not own this post" }));
            }

            var result = await _postService.RemoveAsync(id);

            if (!result.Success)
            {
                return BadRequest(new ErrorViewModel());
            }

            var ptDTO = _mapper.Map<Post, PostDTOout>(result.Resource);
            return Ok(ptDTO);
        }
        
        [HttpPost("vote/{id}", Name = "VotePost")]
        public async Task<IActionResult> Vote(string id)
        {
            var UserId = HttpContext.GetUserId();

            var result = await _postService.Vote(id, UserId);

            if(!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok();
        }

        [HttpDelete("unvote/{id}", Name = "UnVotePost")]
        public async Task<IActionResult> UnVote(string id)
        {
            var UserId = HttpContext.GetUserId();

            var result = await _postService.UnVote(id, UserId);

            if(!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok();
        }
    }
}
