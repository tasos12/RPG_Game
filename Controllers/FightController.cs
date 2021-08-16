using System.Threading.Tasks;
using _NET_Course.Dto.Fight;
using _NET_Course.Models;
using _NET_Course.Services.FightService;
using _NET_Course.Services.Skill;
using Microsoft.AspNetCore.Mvc;

namespace _NET_Course.Cotrollers
{
    [ApiController]
    [Route("[controller]")]
    public class FightController : ControllerBase
    {
        private readonly IFightService _figthService;

        public FightController(IFightService fightService)
        {
            _figthService = fightService;
        }

        [HttpPost("Weapon")]
        public async Task<ActionResult<ServiceResponse<AttackResultDto>>> WeaponAttack(WeaponAttackDto request)
        {
            return Ok(await _figthService.WeaponAttack(request));
        }

        [HttpPost("Skill")]
        public async Task<ActionResult<ServiceResponse<AttackResultDto>>> SkillAttack(SkillAttackDto request)
        {
            return Ok(await _figthService.SkillAttack(request));
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<FightResultDto>>> Fight(FightRequestDto request)
        {
            return Ok(await _figthService.Fight(request));
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<HighScoreDto>>> GetHighscore()
        {
            return Ok(await _figthService.GetHighscore());
        }
    }
}