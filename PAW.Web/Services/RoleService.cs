using APW.Architecture;
using PAW.Architecture.Providers;
using PAW.Models.DTO;

namespace PAW.Web.Services;

public interface IRoleService
{
    Task<IEnumerable<RoleDTO>> GetRolesAsync();
}

public class RoleService : ServiceBase, IRoleService
{
    private const string _path = "Role";
    private readonly IRestProvider _restProvider;

    public RoleService(IRestProvider restProvider)
    {
        _restProvider = restProvider;
    }

    public async Task<IEnumerable<RoleDTO>> GetRolesAsync()
    {
        var response = await _restProvider.GetAsync(
            SetPathUrl(_path),
            id: null
        );

        var roles =
            await JsonProvider.DeserializeAsync<IEnumerable<RoleDTO>>(response);

        return roles;
    }
}