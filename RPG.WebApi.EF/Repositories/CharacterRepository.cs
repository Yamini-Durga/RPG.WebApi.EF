using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RPG.WebApi.EF.Data;
using RPG.WebApi.EF.Dtos;
using RPG.WebApi.EF.Interfaces;
using RPG.WebApi.EF.Models;
using System.Security.Claims;

namespace RPG.WebApi.EF.Repositories
{
    public class CharacterRepository : ICharacterRepository
    {
        private readonly IMapper _mapper;
        private readonly DataContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CharacterRepository(IMapper mapper, DataContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }
        private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
        public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto character)
        {
            Character newCharacter = _mapper.Map<Character>(character);
            newCharacter.User = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == GetUserId());
            _dbContext.Add(newCharacter);
            await _dbContext.SaveChangesAsync();
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            var characters = await _dbContext.Characters.Where(u => u.Id == GetUserId()).ToListAsync();
            serviceResponse.Data = _mapper.Map<List<GetCharacterDto>>(characters);
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            try
            {
                Character deleteCharacter = await _dbContext.Characters
                    .FirstOrDefaultAsync(c => c.Id == id && c.User.Id == GetUserId());
                if (deleteCharacter != null)
                {
                    _dbContext.Characters.Remove(deleteCharacter);
                    await _dbContext.SaveChangesAsync();
                    var characters = _dbContext.Characters.Where(c => c.User.Id == GetUserId()).ToList();
                    serviceResponse.Data = _mapper.Map<List<GetCharacterDto>>(characters);
                }
                else
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Character not found";
                }
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters()
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            var characters = await _dbContext.Characters.Where(u => u.User.Id == GetUserId()).ToListAsync();
            serviceResponse.Data = _mapper.Map<List<GetCharacterDto>>(characters);
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
        {
            var character = await _dbContext.Characters
                .FirstOrDefaultAsync(c => c.Id == id && c.User.Id == GetUserId());
            var serviceResponse = new ServiceResponse<GetCharacterDto>();
            serviceResponse.Data = _mapper.Map<GetCharacterDto>(character);
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto character)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDto>();
            try
            {
                Character updateCharacter = await _dbContext.Characters
                    .Include(c => c.User)
                    .FirstOrDefaultAsync(c => c.Id == character.Id);
                if(updateCharacter != null && updateCharacter.User.Id == GetUserId())
                {
                    _mapper.Map(character, updateCharacter);
                    await _dbContext.SaveChangesAsync();
                    serviceResponse.Data = _mapper.Map<GetCharacterDto>(updateCharacter);
                }
                else
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Character not found";
                }
            }
            catch(Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> AddCharacterSkill(AddCharacterSkillDto characterSkill)
        {
            var response = new ServiceResponse<GetCharacterDto>();
            try
            {
                var character = await _dbContext.Characters
                    .Include(c => c.Weapon)
                    .Include(c => c.Skills)
                    .FirstOrDefaultAsync(c => c.Id == characterSkill.CharacterId &&
                        c.User.Id == GetUserId());
                if(character == null)
                {
                    response.Success = false;
                    response.Message = "Character not found";
                    return response;
                }
                var skill = await _dbContext.Skills.FirstOrDefaultAsync(s => s.Id == characterSkill.SkillId);
                if (skill == null)
                {
                    response.Success = false;
                    response.Message = "Skill not found";
                    return response;
                }
                character.Skills.Add(skill);
                await _dbContext.SaveChangesAsync();
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
