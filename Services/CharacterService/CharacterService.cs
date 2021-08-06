using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _NET_Course.Dto;
using _NET_Course.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace _NET_Course.Services
{
    /// <summary>
    /// A character service implementation that executes actions on the database.
    /// </summary>
    public class CharacterService : ICharacterService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;

        public CharacterService(IMapper mapper, DataContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        /// <summary>
        /// Adds a new character to the database.
        /// </summary>
        /// <param name="character">The character data to be added.</param>
        /// <returns>The list of all characters in the database.</returns>
        public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto character)
        {
            var _serviceResponse = new ServiceResponse<List<GetCharacterDto>>();

            Character _character = _mapper.Map<Character>(character);
            _context.Characters.Add(_character);
            await _context.SaveChangesAsync();
            _serviceResponse.Data = await _context.Characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToListAsync();

            return _serviceResponse;
        }

        /// <summary>
        /// Gets all the characters from the database.
        /// </summary>
        /// <returns>The list of all characters in the database.</returns>
        public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters()
        {
            var _serviceResponse = new ServiceResponse<List<GetCharacterDto>>();

            var _dbCharacters = await _context.Characters.ToListAsync();
            _serviceResponse.Data = _dbCharacters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();

            return _serviceResponse;
        }

        /// <summary>
        /// Gets the character with the specified id.
        /// </summary>
        /// <param name="id">The id of the character searched.</param>
        /// <returns>The character information with the id spesified.</returns>
        public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
        {
            var _serviceResponse = new ServiceResponse<GetCharacterDto>();

            var _dbCharacters = await _context.Characters.FirstOrDefaultAsync(c => c.ID == id);
            _serviceResponse.Data = _mapper.Map<GetCharacterDto>(_dbCharacters);

            return _serviceResponse;
        }
        
        /// <summary>
        /// Updates the character's information with the same id
        /// </summary>
        /// <param name="character">The character to be updated and its information</param>
        /// <returns>The character's updated information, otherwise a not successfull message.</returns>
        public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto character)
        {
            var _serviceResponse = new ServiceResponse<GetCharacterDto>();

            try
            {
                Character _character = await _context.Characters.FirstOrDefaultAsync(c => c.ID == character.ID);

                _character.Name = character.Name;
                _character.HitPoints = character.HitPoints;
                _character.Strength = character.Strength;
                _character.Defence = character.Defence;
                _character.Inteligence = character.Inteligence;
                _character.Class = character.Class;

                await _context.SaveChangesAsync();

                _serviceResponse.Data = _mapper.Map<GetCharacterDto>(_character);
            }
            catch (Exception e)
            {
                _serviceResponse.Success = false;
                _serviceResponse.Message = e.Message;
            }

            return _serviceResponse;
        }

        /// <summary>
        /// Deletes the character with the specified id.
        /// </summary>
        /// <param name="id">The id of the character to be deleted.</param>
        /// <returns>The characters in the database after the deletion, otherwise a not successfull message.</returns>
        public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id)
        {
            var _serviceResponse = new ServiceResponse<List<GetCharacterDto>>();

            try
            {
                Character _character = _context.Characters.FirstOrDefault(c => c.ID == id);
                _context.Characters.Remove(_character);
                await _context.SaveChangesAsync();

                _serviceResponse.Data = _context.Characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
            }
            catch (Exception e)
            {
                _serviceResponse.Success = false;
                _serviceResponse.Message = e.Message;
            }

            return _serviceResponse;
        }
    }
}
