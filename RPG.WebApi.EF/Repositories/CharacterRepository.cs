using AutoMapper;
using RPG.WebApi.EF.Dtos;
using RPG.WebApi.EF.Interfaces;
using RPG.WebApi.EF.Models;

namespace RPG.WebApi.EF.Repositories
{
    public class CharacterRepository : ICharacterRepository
    {
        private readonly IMapper _mapper;
        private List<Character> characters = new List<Character>
        {
            new Character(),
            new Character { Id = 1, Name = "Durga" }
        };
        public CharacterRepository(IMapper mapper)
        {
            _mapper = mapper;
        }
        public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto character)
        {
            Character newCharacter = _mapper.Map<Character>(character);
            newCharacter.Id = characters.Count();
            characters.Add(newCharacter);
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            serviceResponse.Data = _mapper.Map<List<GetCharacterDto>>(characters);
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            try
            {
                Character deleteCharacter = characters.FirstOrDefault(c => c.Id == id);
                if (deleteCharacter != null)
                {
                    characters.Remove(deleteCharacter);
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
            return new ServiceResponse<List<GetCharacterDto>> 
            { 
                Data = _mapper.Map<List<GetCharacterDto>>(characters)
            };
        }

        public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
        {
            var character = characters.FirstOrDefault(c => c.Id == id);
            var serviceResponse = new ServiceResponse<GetCharacterDto>();
            serviceResponse.Data = _mapper.Map<GetCharacterDto>(character);
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto character)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDto>();
            try
            {
                Character updateCharacter = characters.FirstOrDefault(c => c.Id == character.Id);
                if(updateCharacter != null)
                {
                    _mapper.Map(character, updateCharacter);
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
