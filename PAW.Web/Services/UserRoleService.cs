using APW.Architecture;
using PAW.Architecture.Providers;
using PAW.Models.DTO;

namespace PAW.Web.Services;

public interface IUserRoleService
{
    Task<IEnumerable<UserRoleDTO>> GetUserRolesAsync();
    Task<UserRoleDTO> GetUserRoleAsync(decimal id);
    Task SaveUserRoleAsync(UserRoleDTO userRoleDTO);
    Task DeleteUserRoleAsync(decimal id);
}

public class UserRoleService : ServiceBase, IUserRoleService
{
    private const string _path = "UserRole";
    private readonly IRestProvider _restProvider;

    public UserRoleService(IRestProvider restProvider)
    {
        _restProvider = restProvider;
    }

    public async Task<IEnumerable<UserRoleDTO>> GetUserRolesAsync()
    {
        var response = await _restProvider.GetAsync(
            SetPathUrl(_path),
            id: null);

        var userRoles =
            await JsonProvider.DeserializeAsync<IEnumerable<UserRoleDTO>>(response);

        return userRoles;
    }

    public async Task<UserRoleDTO> GetUserRoleAsync(decimal id)
    {
        var response = await _restProvider.GetAsync(
            SetPathUrl(_path + "/" + id),
            id: null);

        return await JsonProvider.DeserializeAsync<UserRoleDTO>(response);
    }

    public async Task SaveUserRoleAsync(UserRoleDTO userRoleDTO)
    {
        PAW.Models.UserRole userRole =
            UserRoleDTO.ConvertTo(userRoleDTO);

        var userRoles = new List<PAW.Models.UserRole>
        {
            userRole
        };

        var content = JsonProvider.Serialize(userRoles);

        await _restProvider.PostAsync(
            SetPathUrl(_path),
            content);
    }

    public async Task DeleteUserRoleAsync(decimal id)
    {
        await _restProvider.DeleteAsync(
            SetPathUrl(_path + "/" + id),
            string.Empty);
    }
}