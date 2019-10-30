using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Forum.Services;
using AutoMapper;
using Forum.DTOin;
using Forum.DTOout;
using Forum.Models;

namespace Forum.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly IMapper _mapper;
        public PostController(IPostService postService, IMapper mapper)
        {
            _postService = postService;
            _mapper = mapper;
        }
        // GET: api/Post
        [HttpGet("get", Name = "GetPosts")]
        [ProducesResponseType(typeof(IEnumerable<PostDTOout>), 200)]
        public async Task<IEnumerable<PostDTOout>> Get()
        {
            var post = await _postService.GetAllAsync();
            var dto = _mapper.Map<IEnumerable<Post>, IEnumerable<PostDTOout>>(post);

            return dto;
        }

        // GET: api/Post/5
        [HttpGet("get/{id}", Name = "GetPost")]
        public async Task<PostDTOout> Get(string id)
        {
            var post = await _postService.GetAsync(id);
            var dto = _mapper.Map<Post, PostDTOout>(post);

            return dto;
        }

        // POST: api/Post
        [HttpPost("post", Name = "PostPost")]
        public async Task<IActionResult> Post([FromBody] PostDTOin post)
        {
            var pt = _mapper.Map<PostDTOin, Post>(post);
            var result = await _postService.AddAsync(pt);

            if (!result.Success)
            {
                return BadRequest(new ErrorViewModel());
            }

            var ptDTO = _mapper.Map<Post, PostDTOout>(result.Resource);
            return Ok(ptDTO);
        }

        // PUT: api/Post/5
        [HttpPut("put/{id}", Name = "PutPost")]
        public async Task<IActionResult> Put(string id, [FromBody] PostDTOin post)
        {
            var pt = _mapper.Map<PostDTOin, Post>(post);
            var result = await _postService.UpdateAsync(id, pt);

            if (!result.Success)
            {
                return BadRequest(new ErrorViewModel());
            }

            var ptDTO = _mapper.Map<Post, PostDTOout>(result.Resource);
            return Ok(ptDTO);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("delete/{id}", Name = "DeletePost")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _postService.RemoveAsync(id);

            if (!result.Success)
            {
                return BadRequest(new ErrorViewModel());
            }

            var ptDTO = _mapper.Map<Post, PostDTOout>(result.Resource);
            return Ok(ptDTO);
        }
    }
}
