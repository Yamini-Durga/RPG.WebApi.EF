using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RPG.WebApi.EF.Data;
using RPG.WebApi.EF.Dtos;
using RPG.WebApi.EF.Interfaces;
using RPG.WebApi.EF.Models;

namespace RPG.WebApi.EF.Repositories
{
    public class CharacterRepository : ICharacterRepository
    {
        private readonly IMapper _mapper;
        private readonly DataContext _dbContext;
        public CharacterRepository(IMapper mapper, DataContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }
        public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto character)
        {
            Character newCharacter = _mapper.Map<Character>(character);
            _dbContext.Add(newCharacter);
            await _dbContext.SaveChangesAsync();
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            var characters = await _dbContext.Characters.ToListAsync();
            serviceResponse.Data = _mapper.Map<List<GetCharacterDto>>(characters);
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            try
            {
                Character deleteCharacter = await _dbContext.Characters.FirstOrDefaultAsync(c => c.Id == id);
                if (deleteCharacter != null)
                {
                    _dbContext.Characters.Remove(deleteCharacter);
                    await _dbContext.SaveChangesAsync();
                    var characters = _dbContext.Characters.ToList();
                    serviceResponse.Data = _mapper.Map<List<GetCharacterDto>>(characters);
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
            var characters = await _dbContext.Characters.ToListAsync();
            serviceResponse.Data = _mapper.Map<List<GetCharacterDto>>(characters);
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
        {
            var character = await _dbContext.Characters.FirstOrDefaultAsync(c => c.Id == id);
            var serviceResponse = new ServiceResponse<GetCharacterDto>();
            serviceResponse.Data = _mapper.Map<GetCharacterDto>(character);
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto character)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDto>();
            try
            {
                Character updateCharacter = await _dbContext.Characters.FirstOrDefaultAsync(c => c.Id == character.Id);
                if(updateCharacter != null)
                {
                    _mapper.Map(character, updateCharacter);
                    await _dbContext.SaveChangesAsync();
                    serviceResponse.Data = _mapper.Map<GetCharacterDto>(updateCharacter);
                }
            }
            catch(Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }
    }
}
