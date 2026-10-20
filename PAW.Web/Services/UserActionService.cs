using APW.Architecture;
using PAW.Architecture.Providers;
using PAW.Models.DTO;

namespace PAW.Web.Services;

public interface IUserActionService
{
    Task<IEnumerable<UserActionDTO>> GetUserActionsAsync();
    Task<UserActionDTO> GetUserActionAsync(decimal id);
    Task SaveUserActionAsync(UserActionDTO userActionDTO);
    Task DeleteUserActionAsync(decimal id);
}

public class UserActionService : ServiceBase, IUserActionService
{
    private const string _path = "UserAction";
    private readonly IRestProvider _restProvider;

    public UserActionService(IRestProvider restProvider)
    {
        _restProvider = restProvider;
    }

    public async Task<IEnumerable<UserActionDTO>> GetUserActionsAsync()
    {
        var response =
            await _restProvider.GetAsync(
                SetPathUrl(_path),
                id: null);

        var userActions =
            await JsonProvider.DeserializeAsync<IEnumerable<UserActionDTO>>(response);

        return userActions;
    }

    public async Task<UserActionDTO> GetUserActionAsync(decimal id)
    {
        var response =
            await _restProvider.GetAsync(
                SetPathUrl(_path + "/" + id),
                id: null);

        var userAction =
            await JsonProvider.DeserializeAsync<UserActionDTO>(response);

        return userAction;
    }

    public async Task SaveUserActionAsync(UserActionDTO userActionDTO)
    {
        PAW.Models.UserAction userAction =
            UserActionDTO.ConvertTo(userActionDTO);

        var userActions = new List<PAW.Models.UserAction>
        {
            userAction
        };

        var content =
            JsonProvider.Serialize(userActions);

        await _restProvider.PostAsync(
            SetPathUrl(_path),
            content);
    }

    public async Task DeleteUserActionAsync(decimal id)
    {
        await _restProvider.DeleteAsync(
            SetPathUrl(_path + "/" + id),
            string.Empty);
    }
}