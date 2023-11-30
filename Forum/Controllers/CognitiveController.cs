using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Forum.BLL.Services;
using Forum.Shared.Contracts.Requests;
using AutoMapper;
using Forum.DAL.Models;
using Forum.Shared.Contracts.Responses;
using System.Threading.Tasks;

namespace Forum.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CognitiveController : ControllerBase
    {
        private ICognitiveService _cognitiveService;
        private IMapper _mapper;

        public CognitiveController(ICognitiveService cognitiveService, IMapper mapper)
        {
            _cognitiveService = cognitiveService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> SpeechToText([FromBody] SpeechToTextRequest speechToTextRequest)
        {
            var result = await _cognitiveService.SpeechToText(speechToTextRequest.wavFile);

            if(!result.Success)
            {
                return BadRequest(result.Message);
            }
            var cognitiveDTO = _mapper.Map<SpeechToText, SpeechToTextResponse>(result.Resource);
            return Ok(cognitiveDTO);
        }
    }
}
