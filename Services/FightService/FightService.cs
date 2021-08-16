using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _NET_Course.Dto.Fight;
using _NET_Course.Models;
using _NET_Course.Services.Skill;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace _NET_Course.Services.FightService
{
    public class FightService : IFightService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public FightService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<FightResultDto>> Fight(FightRequestDto request)
        {
            var _response = new ServiceResponse<FightResultDto>
            {
                Data = new FightResultDto()
            };

            try
            {
                var _characters = await _context.Characters
                                    .Include(c => c.Weapon)
                                    .Include(c => c.Skills)
                                    .Where(c => request.CharacterIds.Contains(c.ID)).ToListAsync();

                bool _defeated = false;
                while(!_defeated)
                {
                    foreach (var _attacker in _characters)
                    {
                        var _opponents = _characters.Where(c => c.ID != _attacker.ID).ToList();
                        var _opponent = _opponents[new Random().Next(_opponents.Count)];

                        int _damage = 0;
                        string _attackUsed = string.Empty;

                        bool _useWeapon = new Random().Next(2) == 0;
                        if(_useWeapon)
                        {
                            _attackUsed = _attacker.Weapon.Name;
                            _damage = DoWeaponAttack(_attacker, _opponent);
                        }
                        else
                        {
                            var _skill = _attacker.Skills[new Random().Next(_attacker.Skills.Count)];
                            _attackUsed = _skill.Name;
                            _damage = DoSkillAttack(_attacker, _opponent, _skill);
                        }

                        _response.Data.Log.Add($"{_attacker.Name} attacks {_opponent.Name} using {_attackUsed} with {(_damage >= 0 ? _damage :0)} damage.");

                        if(_opponent.HitPoints <= 0)
                        {
                            _defeated = true;
                            _attacker.Victories++;
                            _opponent.Defeats++;
                            _response.Data.Log.Add($"{_opponent.Name} has been defeated.");
                            _response.Data.Log.Add($"{_attacker.Name} wins with {_attacker.HitPoints} HP left!");
                            break;
                        }
                    }
                }

                _characters.ForEach
                (
                    c => 
                    {
                        c.Fights++;
                        c.HitPoints = 100;
                    }
                );

                await _context.SaveChangesAsync();
            }
            catch(Exception e)
            {
                _response.Success = false;
                _response.Message = e.Message;
            }

            return _response;
        }

        public async Task<ServiceResponse<AttackResultDto>> SkillAttack(SkillAttackDto request)
        {
            var _response = new ServiceResponse<AttackResultDto>();

            try
            {
                var _attacker = await _context.Characters
                                        .Include(c => c.Skills)
                                        .FirstOrDefaultAsync(c => c.ID == request.AttackerId);

                var _opponent = await _context.Characters
                                        .FirstOrDefaultAsync(c => c.ID == request.OpponentId);

                var _skill = _attacker.Skills.FirstOrDefault(s => s.ID == request.SkillId);
                if (_skill == null)
                {
                    _response.Success = false;
                    _response.Message = $"{_attacker.Name} doesn't know this skill.";
                }

                int _damage = DoSkillAttack(_attacker, _opponent, _skill);

                if (_opponent.HitPoints <= 0)
                {
                    _response.Message = $"{_opponent.Name} has been defeated!";
                }

                await _context.SaveChangesAsync();

                _response.Data = new AttackResultDto
                {
                    Attacker = _attacker.Name,
                    AttackerHP = _attacker.HitPoints,
                    Opponent = _opponent.Name,
                    OpponentHP = _opponent.HitPoints,
                    Damage = _damage
                };
            }
            catch (Exception e)
            {
                _response.Success = false;
                _response.Message = e.Message;
            }

            return _response;
        }

        public async Task<ServiceResponse<AttackResultDto>> WeaponAttack(WeaponAttackDto request)
        {
            var _response = new ServiceResponse<AttackResultDto>();
            
            try
            {
                var _attacker = await _context.Characters
                                        .Include(c => c.Weapon)
                                        .FirstOrDefaultAsync(c => c.ID == request.AttackerId);
                
                var _opponent = await _context.Characters.FirstOrDefaultAsync(c => c.ID == request.OpponentId);

                int _damage = DoWeaponAttack(_attacker, _opponent);

                if(_opponent.HitPoints <= 0)
                {
                    _response.Message = $"{_opponent.Name} has been defeated!";
                }

                await _context.SaveChangesAsync();

                _response.Data = new AttackResultDto
                {
                    Attacker = _attacker.Name,
                    AttackerHP = _attacker.HitPoints,
                    Opponent = _opponent.Name,
                    OpponentHP = _opponent.HitPoints,
                    Damage = _damage
                };
            }
            catch (Exception e)
            {
                _response.Success = false;
                _response.Message = e.Message;
            }

            return _response;
        }


        public async Task<ServiceResponse<List<HighScoreDto>>> GetHighscore()
        {
            var _characters = await _context.Characters
                                    .Where(c => c.Fights > 0)
                                    .OrderByDescending(c => c.Victories)
                                    .ThenBy(c => c.Defeats)
                                    .ToListAsync();

            var _response = new ServiceResponse<List<HighScoreDto>>
            {
                Data = _characters.Select(c => _mapper.Map<HighScoreDto>(c)).ToList()
            };

            return _response;
        }

        private int DoWeaponAttack(Character attacker, Character opponent)
        {
            int _damage = attacker.Weapon.Damage + (new Random().Next(attacker.Strength));

            _damage -= new Random().Next(opponent.Defence);

            if (_damage > 0)
            {
                opponent.HitPoints -= _damage;
            }

            return _damage;
        }

        private int DoSkillAttack(Character attacker, Character opponent, Models.Skill skill)
        {
            int _damage = skill.Damage + (new Random().Next(attacker.Inteligence));
            _damage -= new Random().Next(opponent.Defence);

            if (_damage > 0)
            {
                opponent.HitPoints -= _damage;
            }

            return _damage;
        }
    }
}