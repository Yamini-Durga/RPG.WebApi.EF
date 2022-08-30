using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RPG.WebApi.EF.Dtos;
using RPG.WebApi.EF.Interfaces;
using RPG.WebApi.EF.Models;
using System.Security.Claims;

namespace RPG.WebApi.EF.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CharacterController : ControllerBase
    {
        private readonly ICharacterRepository _characterRepository;

        public CharacterController(ICharacterRepository characterRepository)
        {
            _characterRepository = characterRepository;
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> GetById(int id)
        {
            var character = await _characterRepository.GetCharacterById(id);
            return Ok(character);
        }
        //[AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> GetAll()
        {
            int userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            return Ok(await _characterRepository.GetAllCharacters());
        }
        [HttpPost]
        public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> AddCharacter(AddCharacterDto character)
        {
            return Ok(await _characterRepository.AddCharacter(character));
        }
        [HttpPut]
        public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> UpdateCharacter(UpdateCharacterDto character)
        {
            var response = await _characterRepository.UpdateCharacter(character);
            if(response.Data == null)
            {
                return NotFound(response);
            }
            return Ok(response);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> DeleteCharacter(int id)
        {
            var response = await _characterRepository.DeleteCharacter(id);
            if (response.Data == null)
            {
                return NotFound(response);
            }
            return Ok(response);
        }
        [HttpPost("skill")]
        public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> AddCharacterSkill(AddCharacterSkillDto characterSkill)
        {
            return Ok(await _characterRepository.AddCharacterSkill(characterSkill));
        }
    }
}
