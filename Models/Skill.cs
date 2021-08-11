using System.Collections.Generic;

namespace _NET_Course.Models
{
    public class Skill
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Damage { get; set; }
        public List<Character> Characters { get; set; }
    }
}