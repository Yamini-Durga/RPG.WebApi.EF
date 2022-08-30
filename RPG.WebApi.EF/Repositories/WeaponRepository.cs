using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RPG.WebApi.EF.Data;
using RPG.WebApi.EF.Dtos;
using RPG.WebApi.EF.Interfaces;
using RPG.WebApi.EF.Models;
using System.Security.Claims;

namespace RPG.WebApi.EF.Repositories
{
    public class WeaponRepository : IWeaponRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public WeaponRepository(DataContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<ServiceResponse<GetCharacterDto>> AddWeapon(AddWeaponDto weapon)
        {
            ServiceResponse<GetCharacterDto> response = new ServiceResponse<GetCharacterDto>();
            try
            {
                Character character = await _context.Characters
                    .FirstOrDefaultAsync(c => c.Id == weapon.CharacterId &&
                     c.User.Id == int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
                if(character == null)
                {
                    response.Success = false;
                    response.Message = "Character not found";
                    return response;
                }
                Weapon weaponObj = new Weapon
                {
                    Name = weapon.Name,
                    Damage = weapon.Damage,
                    Character = character,
                    CharacterId = character.Id
                };
                _context.Weapons.Add(weaponObj);
                await _context.SaveChangesAsync();
                response.Data = _mapper.Map<GetCharacterDto>(character);
            }
            catch(Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }
    }
}
