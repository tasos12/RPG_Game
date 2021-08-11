using System.Threading.Tasks;
using _NET_Course.Dto.Character;
using _NET_Course.Dto.Weapon;
using _NET_Course.Models;
using _NET_Course.Services.WeaponService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace _NET_Course.Cotrollers
{

    [Authorize]
    [ApiController]
    [Route("Weapon")]
    public class WeaponController : ControllerBase
    {
        private readonly IWeaponService _weaponService;

        public WeaponController(IWeaponService weaponService)
        {
            _weaponService = weaponService;
        }

        [HttpPost("AddWeapon")]
        public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> AddWeapon(AddWeaponDto newWeapon)
        {
            return Ok(await _weaponService.AddWeapon(newWeapon));
        }
    }
}