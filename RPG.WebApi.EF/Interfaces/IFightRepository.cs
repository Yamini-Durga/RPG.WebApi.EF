using RPG.WebApi.EF.Dtos;
using RPG.WebApi.EF.Models;

namespace RPG.WebApi.EF.Interfaces
{
    public interface IFightRepository
    {
        Task<ServiceResponse<AttackResultDto>> WeaponAttack(WeaponAttackDto request);
        Task<ServiceResponse<AttackResultDto>> SkillAttack(SkillAttackDto request);
        Task<ServiceResponse<FightResultDto>> Fight(FightRequestDto request);
    }
}
