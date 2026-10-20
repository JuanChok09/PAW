using APW.Architecture;
using PAW.Architecture.Providers;
using PAW.Models.DTO;

namespace PAW.Web.Services;

public interface INotificationService
{
    Task<IEnumerable<NotificationDTO>> GetNotificationsAsync();
    Task<NotificationDTO> GetNotificationAsync(int id);
    Task SaveNotificationAsync(NotificationDTO notificationDTO);
    Task DeleteNotificationAsync(int id);
}

public class NotificationService : ServiceBase, INotificationService
{
    private const string _path = "Notification";
    private readonly IRestProvider _restProvider;

    public NotificationService(IRestProvider restProvider)
    {
        _restProvider = restProvider;
    }

    public async Task<IEnumerable<NotificationDTO>> GetNotificationsAsync()
    {
        var response =
            await _restProvider.GetAsync(
                SetPathUrl(_path),
                id: null);

        var notifications =
            await JsonProvider.DeserializeAsync<IEnumerable<NotificationDTO>>(
                response);

        return notifications;
    }

    public async Task<NotificationDTO> GetNotificationAsync(int id)
    {
        var response =
            await _restProvider.GetAsync(
                SetPathUrl(_path + "/" + id),
                id: null);

        var notification =
            await JsonProvider.DeserializeAsync<NotificationDTO>(response);

        return notification;
    }

    public async Task SaveNotificationAsync(NotificationDTO notificationDTO)
    {
        PAW.Models.Notification notification =
            NotificationDTO.ConvertTo(notificationDTO);

        var notifications = new List<PAW.Models.Notification>
        {
            notification
        };

        var content =
            JsonProvider.Serialize(notifications);

        await _restProvider.PostAsync(
            SetPathUrl(_path),
            content);
    }

    public async Task DeleteNotificationAsync(int id)
    {
        await _restProvider.DeleteAsync(
            SetPathUrl(_path + "/" + id),
            string.Empty);
    }
}