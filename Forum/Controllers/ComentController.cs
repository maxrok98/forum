using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Forum.Models;
using Forum.DTOin;
using Forum.DTOout;
using Forum.Services;
using AutoMapper;

namespace Forum.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
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
        public async Task<IEnumerable<ComentDTOout>> Get()
        {
            var coments = await _comentService.GetAllAsync();
            var dto = _mapper.Map<IEnumerable<Coment>, IEnumerable<ComentDTOout>>(coments);

            return dto;
        }

        // GET: api/Coment/5
        [HttpGet("get/{id}", Name = "GetComent")]
        public async Task<ComentDTOout> Get(string id)
        {
            var coment = await _comentService.GetAsync(id);
            var dto = _mapper.Map<Coment, ComentDTOout>(coment);

            return dto;
        }

        // POST: api/Coment
        [HttpPost("post", Name = "PostComent")]
        public async Task<IActionResult> Post([FromBody] ComentDTOin coment)
        {
            var cm = _mapper.Map<ComentDTOin, Coment>(coment);
            var result = await _comentService.AddAsync(cm);

            if (!result.Success)
            {
                return BadRequest(new ErrorViewModel());
            }

            var cmDTO = _mapper.Map<Coment, ComentDTOout>(result.Resource);
            return Ok(cmDTO);
        }

        // PUT: api/Coment/5
        [HttpPut("put/{id}", Name = "PutComent")]
        public async Task<IActionResult> Put(string id, [FromBody] ComentDTOin coment)
        {
            var cm = _mapper.Map<ComentDTOin, Coment>(coment);
            var result = await _comentService.UpdateAsync(id, cm);

            if (!result.Success)
            {
                return BadRequest(new ErrorViewModel());
            }

            var cmDTO = _mapper.Map<Coment, ComentDTOout>(result.Resource);
            return Ok(cmDTO);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("delete/{id}", Name = "DeleteComent")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _comentService.RemoveAsync(id);

            if (!result.Success)
            {
                return BadRequest(new ErrorViewModel());
            }

            var cmDTO = _mapper.Map<Coment, ComentDTOout>(result.Resource);
            return Ok(cmDTO);
        }
    }
}
