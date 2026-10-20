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
            var userRoles =
                await userRoleRepository.ReadAsync() ?? [];

            return userRoles.Select(UserRoleDTO.ConvertFrom);
        }

        [HttpGet("{id}", Name = "GetUserRoleById")]
        public async Task<ActionResult<UserRoleDTO>> GetById(decimal id)
        {
            var userRoles =
                await userRoleRepository.ReadAsync();

            var userRole =
                userRoles.FirstOrDefault(x => x.Id == id);

            if (userRole == null)
                return NotFound();

            return UserRoleDTO.ConvertFrom(userRole);
        }

        [HttpPost]
        public async Task<bool> Save(
            [FromBody] IEnumerable<UserRole> userRoles)
        {
            foreach (var userRole in userRoles)
            {
                var existingItems =
                    await userRoleRepository.ReadAsync();

                var existing =
                    existingItems.FirstOrDefault(
                        x => x.Id == userRole.Id);

                if (existing != null)
                {
                    existing.RoldId = userRole.RoldId;
                    existing.UserId = userRole.UserId;

                    await userRoleRepository.UpdateAsync(existing);
                }
                else
                {
                    await userRoleRepository.CreateAsync(userRole);
                }
            }

            return true;
        }

        [HttpDelete("{id}")]
        public async Task<bool> Delete(decimal id)
        {
            var userRoles =
                await userRoleRepository.ReadAsync();

            var userRole =
                userRoles.FirstOrDefault(x => x.Id == id);

            if (userRole == null)
                return false;

            return await userRoleRepository.DeleteAsync(userRole);
        }
    }
}