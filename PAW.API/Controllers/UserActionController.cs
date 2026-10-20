using Microsoft.AspNetCore.Mvc;
using PAW.DataAccess.Repositories;
using PAW.Models;
using PAW.Models.DTO;

namespace PAW.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserActionController(
        ILogger<UserActionController> logger,
        IUserActionRepository userActionRepository) : ControllerBase
    {
        [HttpGet(Name = "GetUserActions")]
        public async Task<IEnumerable<UserActionDTO>> GetAll()
        {
            var userActions =
                await userActionRepository.ReadAsync() ?? [];

            return userActions.Select(UserActionDTO.ConvertFrom);
        }

        [HttpGet("{id}", Name = "GetUserActionById")]
        public async Task<ActionResult<UserActionDTO>> GetById(decimal id)
        {
            var userActions =
                await userActionRepository.ReadAsync();

            var userAction =
                userActions.FirstOrDefault(x => x.Id == id);

            if (userAction == null)
                return NotFound();

            return UserActionDTO.ConvertFrom(userAction);
        }

        [HttpPost]
        public async Task<bool> Save(
            [FromBody] IEnumerable<UserAction> userActions)
        {
            foreach (var userAction in userActions)
            {
                var existingItems =
                    await userActionRepository.ReadAsync();

                var existing =
                    existingItems.FirstOrDefault(
                        x => x.Id == userAction.Id);

                if (existing != null)
                {
                    existing.Name = userAction.Name;
                    existing.Description = userAction.Description;

                    await userActionRepository.UpdateAsync(existing);
                }
                else
                {
                    await userActionRepository.CreateAsync(userAction);
                }
            }

            return true;
        }

        [HttpDelete("{id}")]
        public async Task<bool> Delete(decimal id)
        {
            var userActions =
                await userActionRepository.ReadAsync();

            var userAction =
                userActions.FirstOrDefault(x => x.Id == id);

            if (userAction == null)
                return false;

            return await userActionRepository.DeleteAsync(userAction);
        }
    }
}