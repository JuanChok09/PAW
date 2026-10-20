using APW.Architecture;
using PAW.Architecture.Providers;
using PAW.Models.DTO;

namespace PAW.Web.Services;

public interface IRoleService
{
    Task<IEnumerable<RoleDTO>> GetRolesAsync();
    Task<RoleDTO> GetRoleAsync(int id);
    Task SaveRoleAsync(RoleDTO roleDTO);
    Task DeleteRoleAsync(int id);
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
        var response =
            await _restProvider.GetAsync(
                SetPathUrl(_path),
                id: null);

        var roles =
            await JsonProvider.DeserializeAsync<IEnumerable<RoleDTO>>(response);

        return roles;
    }

    public async Task<RoleDTO> GetRoleAsync(int id)
    {
        var response =
            await _restProvider.GetAsync(
                SetPathUrl(_path + "/" + id),
                id: null);

        var role =
            await JsonProvider.DeserializeAsync<RoleDTO>(response);

        return role;
    }

    public async Task SaveRoleAsync(RoleDTO roleDTO)
    {
        PAW.Models.Role role =
            RoleDTO.ConvertTo(roleDTO);

        var roles = new List<PAW.Models.Role>
        {
            role
        };

        var content =
            JsonProvider.Serialize(roles);

        await _restProvider.PostAsync(
            SetPathUrl(_path),
            content);
    }

    public async Task DeleteRoleAsync(int id)
    {
        await _restProvider.DeleteAsync(
            SetPathUrl(_path + "/" + id),
            string.Empty);
    }
}