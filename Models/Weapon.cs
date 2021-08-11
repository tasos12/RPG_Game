namespace _NET_Course.Models
{
    public class Weapon
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Damage { get; set; }
        public WeaponType Type { get; set; }
        public Character Character { get; set; }
        public int CharacterID { get; set; }
    }
}