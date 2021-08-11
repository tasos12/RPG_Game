using System;
using System.Security.Claims;
using System.Threading.Tasks;
using _NET_Course.Dto.Character;
using _NET_Course.Dto.Weapon;
using _NET_Course.Models;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace _NET_Course.Services.WeaponService
{
    public class WeaponService : IWeaponService
    {
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public WeaponService(DataContext context, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<GetCharacterDto>> AddWeapon(AddWeaponDto newWeapon)
        {
            var _serviceResponse = new ServiceResponse<GetCharacterDto>();

            try
            {
                var _character = await _context.Characters
                                        .FirstOrDefaultAsync(c => c.ID == newWeapon.CharacterID && 
                                        c.User.ID == int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
                
                if(_character == null)
                {
                    _serviceResponse.Success = false;
                    _serviceResponse.Message = "Character not found.";

                    return _serviceResponse;
                }

                var _weapon = new Weapon
                {
                    Name = newWeapon.Name,
                    Damage = newWeapon.Damage,
                    
                    Character = _character
                };

                _context.Weapon.Add(_weapon);
                await _context.SaveChangesAsync();

                _serviceResponse.Data = _mapper.Map<GetCharacterDto>(_character);
            }
            catch(Exception e)
            {
                _serviceResponse.Success = false;
                _serviceResponse.Message = e.Message;
            }

            return _serviceResponse;
        }
    }
}