using Microsoft.AspNetCore.Mvc;
using PAW.DataAccess.Repositories;
using PAW.Models;
using PAW.Models.DTO;

namespace PAW.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RoleController(
        ILogger<RoleController> logger,
        IRoleRepository roleRepository) : ControllerBase
    {
        [HttpGet(Name = "GetRoles")]
        public async Task<IEnumerable<RoleDTO>> GetAll()
        {
            var roles = await roleRepository.ReadAsync() ?? [];
            return roles.Select(RoleDTO.ConvertFrom);
        }

        [HttpGet("{id:int}", Name = "GetRoleById")]
        public async Task<ActionResult<RoleDTO>> GetById(int id)
        {
            var role = await roleRepository.FindAsync(id);

            if (role == null)
                return NotFound();

            return RoleDTO.ConvertFrom(role);
        }

        [HttpPost]
        public async Task<bool> Save([FromBody] IEnumerable<Role> roles)
        {
            foreach (var role in roles)
            {
                if (role.RoleId > 0)
                    await roleRepository.UpdateAsync(role);
                else
                    await roleRepository.CreateAsync(role);
            }

            return true;
        }

        [HttpDelete]
        public async Task<bool> Delete(Role role)
        {
            return await roleRepository.DeleteAsync(role);
        }
    }
}