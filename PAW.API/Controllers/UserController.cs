using Microsoft.AspNetCore.Mvc;
using PAW.DataAccess.Repositories;
using PAW.Models;
using PAW.Models.DTO;

namespace PAW.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController(
        ILogger<UserController> logger,
        IUserRepository userRepository) : ControllerBase
    {
        [HttpGet(Name = "GetUsers")]
        public async Task<IEnumerable<UserDTO>> GetAll()
        {
            var users =
                await userRepository.ReadAsync() ?? [];

            return users.Select(UserDTO.ConvertFrom);
        }

        [HttpGet("{id:int}", Name = "GetUserById")]
        public async Task<ActionResult<UserDTO>> GetById(int id)
        {
            var user =
                await userRepository.FindAsync(id);

            if (user == null)
                return NotFound();

            return UserDTO.ConvertFrom(user);
        }

        [HttpPost]
        public async Task<bool> Save(
            [FromBody] IEnumerable<User> users)
        {
            foreach (var user in users)
            {
                if (user.UserId > 0)
                {
                    var existingUser =
                        await userRepository.FindAsync(user.UserId);

                    if (existingUser == null)
                        continue;

                    existingUser.Username = user.Username;
                    existingUser.Email = user.Email;
                    existingUser.IsActive = user.IsActive;
                    existingUser.LastModified = user.LastModified;
                    existingUser.ModifiedBy = user.ModifiedBy;
                    existingUser.RoleId = user.RoleId;
                    existingUser.LastModifiedBy = user.LastModifiedBy;

                    await userRepository.UpdateAsync(existingUser);
                }
                else
                {
                    await userRepository.CreateAsync(user);
                }
            }

            return true;
        }

        [HttpDelete("{id:int}")]
        public async Task<bool> Delete(int id)
        {
            var user =
                await userRepository.FindAsync(id);

            if (user == null)
                return false;

            return await userRepository.DeleteAsync(user);
        }
    }
}