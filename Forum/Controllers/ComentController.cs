using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Forum.Extensions;
using Forum.DAL.Models;
using Forum.BLL.Services;
using Forum.Shared.Contracts.Requests;
using Forum.Shared.Contracts.Responses;
using Forum.Shared.Services;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text.Json;

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
        private readonly DiffieHellman _diffieHellman;
        public ComentController(IComentService comentService, IMapper mapper, DiffieHellman diffieHellman)
        {
            _comentService = comentService;
            _mapper = mapper;
            _diffieHellman = diffieHellman;
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
        public async Task<IActionResult> Post([FromBody] byte[] comentArray)
        {
            long usersPublicKey = long.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == "publicKey").Value);
            byte[] usersIV = Convert.FromBase64String(HttpContext.User.Claims.FirstOrDefault(x => x.Type == "IV").Value);
            string decryptedComent = _diffieHellman.Decrypt(usersPublicKey, comentArray, usersIV);
            ComentRequest coment = JsonSerializer.Deserialize<ComentRequest>(decryptedComent);
            var cm = _mapper.Map<ComentRequest, Coment>(coment);
            cm.UserId = HttpContext.GetUserId();
            var result = await _comentService.AddAsync(cm);

            if (!result.Success)
            {
                return BadRequest(result.Message);
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
                return BadRequest("You do not own this coment");
            }
            var cm = _mapper.Map<ComentRequest, Coment>(coment);
            var result = await _comentService.UpdateAsync(id, cm);

            if (!result.Success)
            {
                return BadRequest(result.Message);
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
                return BadRequest("You do not own this coment");
            }
            var result = await _comentService.RemoveAsync(id);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            var cmDTO = _mapper.Map<Coment, ComentResponse>(result.Resource);
            return Ok(cmDTO);
        }
    }
}
