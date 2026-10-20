using APW.Architecture;
using PAW.Architecture.Providers;
using PAW.Models.DTO;

namespace PAW.Web.Services;

public interface ITaskService
{
    Task<IEnumerable<TaskDTO>> GetTasksAsync();
    Task<TaskDTO> GetTaskAsync(int id);
    Task SaveTaskAsync(TaskDTO taskDTO);
    Task DeleteTaskAsync(int id);
}

public class TaskService : ServiceBase, ITaskService
{
    private const string _path = "Task";
    private readonly IRestProvider _restProvider;

    public TaskService(IRestProvider restProvider)
    {
        _restProvider = restProvider;
    }

    public async Task<IEnumerable<TaskDTO>> GetTasksAsync()
    {
        var response =
            await _restProvider.GetAsync(
                SetPathUrl(_path),
                id: null);

        var tasks =
            await JsonProvider.DeserializeAsync<IEnumerable<TaskDTO>>(response);

        return tasks;
    }

    public async Task<TaskDTO> GetTaskAsync(int id)
    {
        var response =
            await _restProvider.GetAsync(
                SetPathUrl(_path + "/" + id),
                id: null);

        var task =
            await JsonProvider.DeserializeAsync<TaskDTO>(response);

        return task;
    }

    public async Task SaveTaskAsync(TaskDTO taskDTO)
    {
        PAW.Models.Task task =
            TaskDTO.ConvertTo(taskDTO);

        var tasks = new List<PAW.Models.Task>
        {
            task
        };

        var content =
            JsonProvider.Serialize(tasks);

        await _restProvider.PostAsync(
            SetPathUrl(_path),
            content);
    }

    public async Task DeleteTaskAsync(int id)
    {
        await _restProvider.DeleteAsync(
            SetPathUrl(_path + "/" + id),
            string.Empty);
    }
}