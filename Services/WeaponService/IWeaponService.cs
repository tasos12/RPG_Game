
using System.Threading.Tasks;
using _NET_Course.Models;
using _NET_Course.Dto.Weapon;
using _NET_Course.Dto.Character;

namespace _NET_Course.Services.WeaponService
{
    public interface IWeaponService
    {
        Task<ServiceResponse<GetCharacterDto>> AddWeapon(AddWeaponDto newWeapon);
    }
}
