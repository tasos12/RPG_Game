using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class CharacterController : ControllerBase
{
    private readonly ICharacterService _characterService;

    public CharacterController(ICharacterService characterService)
    {
        _characterService = characterService;
    }

    [HttpGet]
    [Route("GetAll")]
    public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> Get()
    {
        return Ok(await _characterService.GetAllCharacters());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> GetSingle(int id) 
    {
        var _response = await _characterService.GetCharacterById(id);
        if (_response.Data == null)
        {
            return NotFound(_response);
        }
        return Ok(_response);
    }

    [HttpPost]
    public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> AddCharacter(AddCharacterDto character)
    {
        return Ok(await _characterService.AddCharacter(character));
    }

    [HttpPut]
    public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> UpdateCharacter(UpdateCharacterDto character)
    {
        var _response = await _characterService.UpdateCharacter(character);
        if(_response.Data == null)
        {
            return NotFound(_response);
        }

        return Ok(_response);
    }

    [HttpDelete("id")]
    public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> DeleteCharacter(int id)
    {
        var _response = await _characterService.DeleteCharacter(id);
        if (_response.Data == null)
        {
            return NotFound(_response);
        }

        return Ok(_response);
    }
}