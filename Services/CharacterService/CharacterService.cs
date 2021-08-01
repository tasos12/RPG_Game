using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

public class CharacterService : ICharacterService
{
    private static List<Character> characters = new List<Character> {
        new Character(),
        new Character { ID = 1, Name = "Sam" }
    };

    private readonly IMapper _mapper;

    public CharacterService(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto character)
    {
        var _serviceResponse = new ServiceResponse<List<GetCharacterDto>>();

        Character _character = _mapper.Map<Character>(character);
        _character.ID = characters.Max(c => c.ID) + 1;
        characters.Add(_character);   
        _serviceResponse.Data = characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();

        return _serviceResponse;
    }

    public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters()
    {
        var _serviceResponse = new ServiceResponse<List<GetCharacterDto>>();

        _serviceResponse.Data = characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();

        return _serviceResponse;
    }

    public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
    {
        var _serviceResponse = new ServiceResponse<GetCharacterDto>();

        _serviceResponse.Data = _mapper.Map<GetCharacterDto>(characters.FirstOrDefault(c => c.ID == id));

        return _serviceResponse;
    }

    public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto character)
    {
        var _serviceResponse = new ServiceResponse<GetCharacterDto>();

        try
        {
            Character _character = characters.FirstOrDefault(c => c.ID == character.ID);
            _character.Name = character.Name;
            _character.HitPoints = character.HitPoints;
            _character.Strength = character.Strength;
            _character.Defence = character.Defence;
            _character.Inteligence = character.Inteligence;
            _character.Class = character.Class;
            _serviceResponse.Data = _mapper.Map<GetCharacterDto>(_character);
        }
        catch (Exception e)
        {
            _serviceResponse.Success = false;
            _serviceResponse.Message = e.Message;
        }

        return _serviceResponse;
    }

    public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id)
    {
        var _serviceResponse = new ServiceResponse<List<GetCharacterDto>>();

        try
        {
            Character _character = characters.FirstOrDefault(c => c.ID == id);
            characters.Remove(_character);
            _serviceResponse.Data = characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
        }
        catch (Exception e)
        {
            _serviceResponse.Success = false;
            _serviceResponse.Message = e.Message;
        }

        return _serviceResponse;
    }

}