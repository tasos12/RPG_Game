using System.Collections.Generic;
using System.Threading.Tasks;
using _NET_Course.Dto.Character;
using _NET_Course.Models;
using _NET_Course.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// An API endpoint for characters
/// </summary>
[Authorize]
[ApiController]
[Route("[controller]")]
public class CharacterController : ControllerBase
{
    private readonly ICharacterService _characterService;

    public CharacterController(ICharacterService characterService)
    {
        _characterService = characterService;
    }

    /// <summary>
    /// Gets all the characters using the CharacterService and returns an HTTP result.
    /// </summary>
    /// <returns> An 200 OK status code and the data of all characters.</returns>
    [HttpGet]
    [Route("GetAll")]
    public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> Get()
    {
        return Ok(await _characterService.GetAllCharacters());
    }

    /// <summary>
    /// Searches and returns the character with the specified id as an HTTP result.
    /// </summary>
    /// <param name="id">The searched chracters id.</param>
    /// <returns>An 200 OK status code with the character information or 404 NOT FOUND if the id doesn't match any in the database.</returns>
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

    /// <summary>
    /// Adds a character to the database using the CharacterService.
    /// </summary>
    /// <param name="character">The Character dto with the information.</param>
    /// <returns>An 200 OK status code when the character is added.</returns>
    [HttpPost]
    public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> AddCharacter(AddCharacterDto character)
    {
        return Ok(await _characterService.AddCharacter(character));
    }

    /// <summary>
    /// Updates a characters information in the database using the CharacterService.
    /// </summary>
    /// <param name="character">The updated character's information.</param>
    /// <returns>An 200 OK status code if the character is updated succesfully, otherwise an An 404 NOT FOUND if the character is not found.</returns>
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

    /// <summary>
    /// Deletes a character from the database using the CharacterService.
    /// </summary>
    /// <param name="id">The character's id to be deleted.</param>
    /// <returns>An 200 OK status code if the character is updated succesfully, otherwise an An 404 NOT FOUND if the character is not found.</returns>
    [HttpDelete("{id}")]
    public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> DeleteCharacter(int id)
    {
        var _response = await _characterService.DeleteCharacter(id);
        if (_response.Data == null)
        {
            return NotFound(_response);
        }

        return Ok(_response);
    }

    [HttpPost("Skill")]
    public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> AddCharacterSkill(AddCharacterSkillDto newSkill)
    {
        return Ok(await _characterService.AddCharacterSkill(newSkill));
    }
}