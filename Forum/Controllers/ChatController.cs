using AutoMapper;
using Forum.Models;
using Forum.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Forum.Contracts.Requests;
using Forum.Contracts.Responses;
using Forum.Extensions;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Forum.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;
        private readonly IMapper _mapper;

        public ChatController(IChatService chatService, IMapper mapper)
        {
            _chatService = chatService;
            _mapper = mapper;
        }
        // GET: api/<ChatsController>
        [HttpGet("get")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Get()
        {
            var result = await _chatService.GetAllAsync();
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            var dto = _mapper.Map<IEnumerable<Chat>, IEnumerable<ChatShortResponse>>(result.Resource);

            return Ok(dto);
        }

        [HttpGet("get-my-chats")]
        public async Task<IActionResult> GetMyChats()
        {
            var userId = HttpContext.GetUserId();
            var result = await _chatService.GetAllFromUserAsync(userId);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            var dto = _mapper.Map<IEnumerable<Chat>, IEnumerable<ChatShortResponse>>(result.Resource);

            return Ok(dto);
        }

        // GET api/<ChatsController>/5
        [HttpGet("get/{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var userId = HttpContext.GetUserId();
            var result = await _chatService.GetAsync(id, userId);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            var dto = _mapper.Map<Chat, ChatResponse>(result.Resource);

            return Ok(dto);
        }

        // POST api/<ChatsController>
        [HttpPost("post")]
        public async Task<IActionResult> Post([FromBody] ChatRequest chat)
        {
            var ch = _mapper.Map<ChatRequest, Chat>(chat);
            var result = await _chatService.AddAsync(ch);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            var chDTO = _mapper.Map<Chat, ChatResponse>(result.Resource);
            return Ok(chDTO);
        }

        // PUT api/<ChatsController>/5
        [HttpPut("put/{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] ChatRequest chat)
        {
            var userId = HttpContext.GetUserId();
            var ch = _mapper.Map<ChatRequest, Chat>(chat);
            var result = await _chatService.UpdateAsync(id, userId, ch);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            var chDTO = _mapper.Map<Chat, ChatResponse>(result.Resource);
            return Ok(chDTO);
        }

        // DELETE api/<ChatsController>/5
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var userId = HttpContext.GetUserId();
            var result = await _chatService.RemoveAsync(id, userId);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            var chDTO = _mapper.Map<Chat, ChatResponse>(result.Resource);
            return Ok(chDTO);

        }
    }
}
