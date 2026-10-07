using Microsoft.AspNetCore.Mvc;
using PAW.DataAccess.Repositories;
using PAW.Models;
using PAW.Models.DTO;

namespace PAW.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserRoleController(
        ILogger<UserRoleController> logger,
        IUserRoleRepository userRoleRepository) : ControllerBase
    {
        [HttpGet(Name = "GetUserRoles")]
        public async Task<IEnumerable<UserRoleDTO>> GetAll()
        {
            var userRoles = await userRoleRepository.ReadAsync() ?? [];
            return userRoles.Select(UserRoleDTO.ConvertFrom);
        }

        [HttpGet("{id:int}", Name = "GetUserRoleById")]
        public async Task<ActionResult<UserRoleDTO>> GetById(int id)
        {
            var userRole = await userRoleRepository.FindAsync(id);

            if (userRole == null)
                return NotFound();

            return UserRoleDTO.ConvertFrom(userRole);
        }

        [HttpPost]
        public async Task<bool> Save([FromBody] IEnumerable<UserRole> userRoles)
        {
            foreach (var userRole in userRoles)
            {
                if (userRole.Id.GetValueOrDefault() > 0)
                    await userRoleRepository.UpdateAsync(userRole);
                else
                    await userRoleRepository.CreateAsync(userRole);
            }

            return true;
        }

        [HttpDelete]
        public async Task<bool> Delete(UserRole userRole)
        {
            return await userRoleRepository.DeleteAsync(userRole);
        }
    }
}