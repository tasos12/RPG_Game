using System.Collections.Generic;
using System.Threading.Tasks;
using _NET_Course.Dto.Fight;
using _NET_Course.Models;
using _NET_Course.Services.Skill;

namespace _NET_Course.Services.FightService
{
    public interface IFightService 
    {
        Task<ServiceResponse<AttackResultDto>> WeaponAttack(WeaponAttackDto request);
        Task<ServiceResponse<AttackResultDto>> SkillAttack(SkillAttackDto request);
        Task<ServiceResponse<FightResultDto>> Fight(FightRequestDto requset);
        Task<ServiceResponse<List<HighScoreDto>>> GetHighscore();
    }
}