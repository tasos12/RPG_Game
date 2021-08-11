using _NET_Course.Models;

namespace _NET_Course.Dto.Weapon
{
    public class AddWeaponDto
    {
        public string Name { get; set; }
        public int Damage { get; set; }
        public WeaponType Type { get; set; }
        public int CharacterID { get; set; }
    }
}