using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Forum.Services;
using AutoMapper;
using Forum.Models;
using Forum.Extensions;
using Forum.Contracts.Responses;
using Forum.Contracts.Requests;
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
            var dto = _mapper.Map<IEnumerable<Post>, IEnumerable<PostResponse>>(post);

            if(pagination == null || pagination.PageNumber < 1 || pagination.PageSize < 1)
            {
                return Ok(new PageResponse<PostResponse>(dto));
            }

            int countOfAllPosts = await _postService.GetCountOfAllPostsAsync();
            var paginationResponse = PaginationHelpers.CreatePaginatedResponse(_uriService, pagination, dto, countOfAllPosts, ApiRoutes.Posts.GetAll);

            return Ok(paginationResponse);
        }

        // GET: api/Post/5
        [HttpGet("get/{id}", Name = "GetPost")]
        public async Task<IActionResult> Get(string id)
        {
            var result = await _postService.GetAsync(id);

            if (!result.Success)
            {
                return BadRequest(new ErrorViewModel());
            }
            var dto = _mapper.Map<Post, PostResponse>(result.Resource);

            return Ok(new Response<PostResponse>(dto));
        }

        // POST: api/Post
        [HttpPost("post", Name = "PostPost")]
        public async Task<IActionResult> Post([FromBody] PostRequest post)
        {
            var pt = _mapper.Map<PostRequest, Post>(post);
            pt.UserId = HttpContext.GetUserId();
            var result = await _postService.AddAsync(pt);

            if (!result.Success)
            {
                return BadRequest(new ErrorViewModel());
            }

            var ptDTO = _mapper.Map<Post, PostResponse>(result.Resource);
            return Ok(new Response<PostResponse>(ptDTO));
        }

        // PUT: api/Post/5
        [HttpPut("put/{id}", Name = "PutPost")]
        public async Task<IActionResult> Put(string id, [FromBody] PostRequest post)
        {
            var userOwnsPost = await _postService.UserOwnsPostAsync(id, HttpContext.GetUserId());

            if (!userOwnsPost)
            {
                return BadRequest(new ErrorResponse(new ErrorModel { Message = "You do not own this post" }));
            }

            var pt = _mapper.Map<PostRequest, Post>(post);
            var result = await _postService.UpdateAsync(id, pt);

            if (!result.Success)
            {
                return BadRequest(new ErrorViewModel());
            }

            var ptDTO = _mapper.Map<Post, PostResponse>(result.Resource);
            return Ok(new Response<PostResponse>(ptDTO));
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

            var ptDTO = _mapper.Map<Post, PostResponse>(result.Resource);
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
