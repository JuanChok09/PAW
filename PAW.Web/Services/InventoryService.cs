using APW.Architecture;
using PAW.Architecture.Providers;
using PAW.Models.DTO;

namespace PAW.Web.Services;

public interface IInventoryService
{
    Task<IEnumerable<InventoryDTO>> GetInventoriesAsync();
    Task<InventoryDTO> GetInventoryAsync(int id);
}

public class InventoryService : ServiceBase, IInventoryService
{
    private const string _path = "Inventory";
    private readonly IRestProvider _restProvider;

    public InventoryService(IRestProvider restProvider)
    {
        _restProvider = restProvider;
    }

    public async Task<IEnumerable<InventoryDTO>> GetInventoriesAsync()
    {
        var response = await _restProvider.GetAsync(
            SetPathUrl(_path),
            id: null
        );

        var inventories =
            await JsonProvider.DeserializeAsync<IEnumerable<InventoryDTO>>(response);

        return inventories;
    }

    public async Task<InventoryDTO> GetInventoryAsync(int id)
    {
        var response = await _restProvider.GetAsync(
            SetPathUrl(_path),
            id.ToString()
        );

        var inventory =
            await JsonProvider.DeserializeAsync<InventoryDTO>(response);

        return inventory;
    }
}