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
            var userActions = await userActionRepository.ReadAsync() ?? [];
            return userActions.Select(UserActionDTO.ConvertFrom);
        }

        [HttpGet("{id:int}", Name = "GetUserActionById")]
        public async Task<ActionResult<UserActionDTO>> GetById(int id)
        {
            var userAction = await userActionRepository.FindAsync(id);

            if (userAction == null)
                return NotFound();

            return UserActionDTO.ConvertFrom(userAction);
        }

        [HttpPost]
        public async Task<bool> Save([FromBody] IEnumerable<UserAction> userActions)
        {
            foreach (var userAction in userActions)
            {
                if (userAction.Id.GetValueOrDefault() > 0)
                    await userActionRepository.UpdateAsync(userAction);
                else
                    await userActionRepository.CreateAsync(userAction);
            }

            return true;
        }

        [HttpDelete]
        public async Task<bool> Delete(UserAction userAction)
        {
            return await userActionRepository.DeleteAsync(userAction);
        }
    }
}