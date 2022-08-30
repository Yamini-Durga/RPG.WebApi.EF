using RPG.WebApi.EF.Dtos;
using RPG.WebApi.EF.Models;

namespace RPG.WebApi.EF.Interfaces
{
    public interface IWeaponRepository
    {
        Task<ServiceResponse<GetCharacterDto>> AddWeapon(AddWeaponDto weapon);
    }
}
