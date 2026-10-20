using APW.Architecture;
using PAW.Architecture.Providers;
using PAW.Models.DTO;

namespace PAW.Web.Services;

public interface IUserService
{
    Task<IEnumerable<UserDTO>> GetUsersAsync();
    Task<UserDTO> GetUserAsync(int id);
    Task SaveUserAsync(UserDTO userDTO);
    Task DeleteUserAsync(int id);
}

public class UserService : ServiceBase, IUserService
{
    private const string _path = "User";
    private readonly IRestProvider _restProvider;

    public UserService(IRestProvider restProvider)
    {
        _restProvider = restProvider;
    }

    public async Task<IEnumerable<UserDTO>> GetUsersAsync()
    {
        var response =
            await _restProvider.GetAsync(
                SetPathUrl(_path),
                id: null);

        var users =
            await JsonProvider.DeserializeAsync<IEnumerable<UserDTO>>(response);

        return users;
    }

    public async Task<UserDTO> GetUserAsync(int id)
    {
        var response =
            await _restProvider.GetAsync(
                SetPathUrl(_path + "/" + id),
                id: null);

        var user =
            await JsonProvider.DeserializeAsync<UserDTO>(response);

        return user;
    }

    public async Task SaveUserAsync(UserDTO userDTO)
    {
        PAW.Models.User user =
            UserDTO.ConvertTo(userDTO);

        var users = new List<PAW.Models.User>
        {
            user
        };

        var content =
            JsonProvider.Serialize(users);

        await _restProvider.PostAsync(
            SetPathUrl(_path),
            content);
    }

    public async Task DeleteUserAsync(int id)
    {
        await _restProvider.DeleteAsync(
            SetPathUrl(_path + "/" + id),
            string.Empty);
    }
}