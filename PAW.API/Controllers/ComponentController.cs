using Microsoft.AspNetCore.Mvc;
using PAW.DataAccess.Repositories;
using PAW.Models;
using PAW.Models.DTO;

namespace PAW.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ComponentController(
        ILogger<ComponentController> logger,
        IComponentRepository componentRepository) : ControllerBase
    {
        [HttpGet(Name = "GetComponents")]
        public async Task<IEnumerable<ComponentDTO>> GetAll()
        {
            var components = await componentRepository.ReadAsync() ?? [];

            return components.Select(ComponentDTO.ConvertFrom);
        }

        [HttpGet("{id}", Name = "GetComponentById")]
        public async Task<ActionResult<ComponentDTO>> GetById(decimal id)
        {
            var components = await componentRepository.ReadAsync();

            var component =
                components.FirstOrDefault(x => x.Id == id);

            if (component == null)
                return NotFound();

            return ComponentDTO.ConvertFrom(component);
        }

        [HttpPost]
        public async Task<bool> Save(
            [FromBody] IEnumerable<Component> components)
        {
            foreach (var component in components)
            {
                if (component.Id > 0)
                    await componentRepository.UpdateAsync(component);
                else
                    await componentRepository.CreateAsync(component);
            }

            return true;
        }

        [HttpDelete("{id}")]
        public async Task<bool> Delete(decimal id)
        {
            var components = await componentRepository.ReadAsync();

            var component =
                components.FirstOrDefault(x => x.Id == id);

            if (component == null)
                return false;

            return await componentRepository.DeleteAsync(component);
        }
    }
}