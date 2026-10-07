using PAW.Models;
using PAW.Repositories;

namespace PAW.DataAccess.Repositories;

public interface ITaskRepository : IRepositoryBase<PAW.Models.Task>
{
    Task<bool> UpsertAsync(PAW.Models.Task entity, bool isUpdating);
    Task<bool> CreateAsync(PAW.Models.Task entity);
    Task<bool> DeleteAsync(PAW.Models.Task entity);
    Task<IEnumerable<PAW.Models.Task>> ReadAsync();
    Task<PAW.Models.Task> FindAsync(int id);
    Task<bool> UpdateAsync(PAW.Models.Task entity);
    Task<bool> UpdateManyAsync(IEnumerable<PAW.Models.Task> entities);
    Task<bool> ExistsAsync(PAW.Models.Task entity);
}

public class TaskRepository : RepositoryBase<PAW.Models.Task>, ITaskRepository
{
}