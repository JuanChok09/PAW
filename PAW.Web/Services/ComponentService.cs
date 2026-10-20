using APW.Architecture;
using PAW.Architecture.Providers;
using PAW.Models.DTO;

namespace PAW.Web.Services;

public interface IComponentService
{
    Task<IEnumerable<ComponentDTO>> GetComponentsAsync();
    Task<ComponentDTO> GetComponentAsync(decimal id);
    Task SaveComponentAsync(ComponentDTO componentDTO);
    Task DeleteComponentAsync(decimal id);
}

public class ComponentService : ServiceBase, IComponentService
{
    private const string _path = "Component";
    private readonly IRestProvider _restProvider;

    public ComponentService(IRestProvider restProvider)
    {
        _restProvider = restProvider;
    }

    public async Task<IEnumerable<ComponentDTO>> GetComponentsAsync()
    {
        var response =
            await _restProvider.GetAsync(
                SetPathUrl(_path),
                id: null);

        var components =
            await JsonProvider.DeserializeAsync<IEnumerable<ComponentDTO>>(
                response);

        return components;
    }

    public async Task<ComponentDTO> GetComponentAsync(decimal id)
    {
        var response =
            await _restProvider.GetAsync(
                SetPathUrl(_path + "/" + id),
                id: null);

        var component =
            await JsonProvider.DeserializeAsync<ComponentDTO>(response);

        return component;
    }

    public async Task SaveComponentAsync(ComponentDTO componentDTO)
    {
        PAW.Models.Component component =
            ComponentDTO.ConvertTo(componentDTO);

        var components = new List<PAW.Models.Component>
        {
            component
        };

        var content =
            JsonProvider.Serialize(components);

        await _restProvider.PostAsync(
            SetPathUrl(_path),
            content);
    }

    public async Task DeleteComponentAsync(decimal id)
    {
        await _restProvider.DeleteAsync(
            SetPathUrl(_path + "/" + id),
            string.Empty);
    }
}