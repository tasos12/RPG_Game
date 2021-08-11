
using _NET_Course.Dto.Character;
using _NET_Course.Dto.Skill;
using _NET_Course.Dto.Weapon;
using _NET_Course.Models;
using AutoMapper;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile() 
    {
        CreateMap<Character, GetCharacterDto>();
        CreateMap<AddCharacterDto, Character>();
        CreateMap<Weapon, GetWeaponDto>();
        CreateMap<Skill, GetSkillDto>();
    }
}