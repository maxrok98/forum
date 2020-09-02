using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Forum.Models;
using Forum.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Forum.Extensions;
using Forum.Contracts.Responses;
using Forum.Contracts.Requests;

namespace Forum.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ComentController : ControllerBase
    {
        private readonly IComentService _comentService;
        private readonly IMapper _mapper;
        public ComentController(IComentService comentService, IMapper mapper)
        {
            _comentService = comentService;
            _mapper = mapper;
        }
        // GET: api/Coment
        [HttpGet("get", Name = "GetComents")]
        [AllowAnonymous]
        public async Task<IEnumerable<ComentResponse>> Get()
        {
            var coments = await _comentService.GetAllAsync();
            var dto = _mapper.Map<IEnumerable<Coment>, IEnumerable<ComentResponse>>(coments);

            return dto;
        }

        // GET: api/Coment/5
        [HttpGet("get/{id}", Name = "GetComent")]
        [AllowAnonymous]
        public async Task<ComentResponse> Get(string id)
        {
            var coment = await _comentService.GetAsync(id);
            var dto = _mapper.Map<Coment, ComentResponse>(coment);

            return dto;
        }

        // POST: api/Coment
        [HttpPost("post", Name = "PostComent")]
        public async Task<IActionResult> Post([FromBody] ComentRequest coment)
        {
            var cm = _mapper.Map<ComentRequest, Coment>(coment);
            cm.UserId = HttpContext.GetUserId();
            var result = await _comentService.AddAsync(cm);

            if (!result.Success)
            {
                return BadRequest(new ErrorViewModel());
            }

            var cmDTO = _mapper.Map<Coment, ComentResponse>(result.Resource);
            return Ok(cmDTO);
        }

        // PUT: api/Coment/5
        [HttpPut("put/{id}", Name = "PutComent")]
        public async Task<IActionResult> Put(string id, [FromBody] ComentRequest coment)
        {
            var userOwnsComent = await _comentService.UserOwnsComentAsync(id, HttpContext.GetUserId());

            if (!userOwnsComent)
            {
                return BadRequest(new ErrorResponse(new ErrorModel { Message = "You do not own this coment" }));
            }
            var cm = _mapper.Map<ComentRequest, Coment>(coment);
            var result = await _comentService.UpdateAsync(id, cm);

            if (!result.Success)
            {
                return BadRequest(new ErrorViewModel());
            }

            var cmDTO = _mapper.Map<Coment, ComentResponse>(result.Resource);
            return Ok(cmDTO);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("delete/{id}", Name = "DeleteComent")]
        public async Task<IActionResult> Delete(string id)
        {
            var userOwnsComent = await _comentService.UserOwnsComentAsync(id, HttpContext.GetUserId());

            if (!userOwnsComent)
            {
                return BadRequest(new ErrorResponse(new ErrorModel { Message = "You do not own this coment" }));
            }
            var result = await _comentService.RemoveAsync(id);

            if (!result.Success)
            {
                return BadRequest(new ErrorViewModel());
            }

            var cmDTO = _mapper.Map<Coment, ComentResponse>(result.Resource);
            return Ok(cmDTO);
        }
    }
}
