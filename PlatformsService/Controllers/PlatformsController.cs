using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Models;
using PlatformService.SyncDataServices.Http;

namespace PlatformService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private readonly IPlatformRepo _repository;
        private readonly IMapper _mapper;
        private readonly IHttpCommandDataClient _httpCommandDataClient;

        public PlatformsController(IPlatformRepo repository, IMapper mapper, IHttpCommandDataClient httpCommandDataClient)
        {
            _repository = repository;
            _mapper = mapper;
            _httpCommandDataClient = httpCommandDataClient;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
        {
            var items = _repository.GetAllPlatforms();

            return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(items));
        }

        [HttpGet("{id}", Name = "GetPlatformById")]
        public ActionResult<PlatformReadDto> GetPlatformById(int id)
        {
            var item = _repository.GetPlatform(id);

            if (item != null)
                return Ok(_mapper.Map<PlatformReadDto>(item));
            
            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<PlatformReadDto>> CreatePlatform(PlatformCreateDto platformCreateDto)
        {
            var newPlatform = _mapper.Map<Platform>(platformCreateDto);

            _repository.CreatePlatform(newPlatform);

            _repository.SaveChanges();

            var platformReadDto = _mapper.Map<PlatformReadDto>(newPlatform);

            //try
            //{
            //    await _httpCommandDataClient.SendPlatformToCommand(platformReadDto);
            //}
            //catch (Exception e)
            //{

            //    Console.WriteLine($"Could not send synchronously: {e.Message}");
            //}

            return CreatedAtRoute(nameof(GetPlatformById), new { Id = platformReadDto.Id }, platformReadDto );

        }
    }
}
