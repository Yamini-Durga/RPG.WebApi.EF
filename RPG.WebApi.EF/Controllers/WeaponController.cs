
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RPG.WebApi.EF.Dtos;
using RPG.WebApi.EF.Interfaces;
using RPG.WebApi.EF.Models;

namespace RPG.WebApi.EF.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class WeaponController : ControllerBase
    {
        private readonly IWeaponRepository _repo;

        public WeaponController(IWeaponRepository repo)
        {
            _repo = repo;
        }
        [HttpPost]
        public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> AddWeapon(AddWeaponDto weapon)
        {
            return Ok(await _repo.AddWeapon(weapon));
        }
    }
}
