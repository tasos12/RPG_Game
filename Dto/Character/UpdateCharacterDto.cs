using _NET_Course.Models;

namespace _NET_Course.Dto
{
    public class UpdateCharacterDto
    {
        public int ID { get; set; }
        public string Name { get; set; } = "Dick";
        public int HitPoints { get; set; } = 10;
        public int Strength { get; set; } = 10;
        public int Defence { get; set; } = 10;
        public int Inteligence { get; set; } = 10;
        public RpgClass Class { get; set; } = RpgClass.Knight;
    }
}