using APW.Architecture;
using PAW.Architecture.Providers;
using PAW.Models.DTO;

namespace PAW.Web.Services;

public interface ISupplierService
{
    Task<IEnumerable<SupplierDTO>> GetSuppliersAsync();
    Task<SupplierDTO> GetSupplierAsync(int id);
    Task SaveSupplierAsync(SupplierDTO supplierDTO);
    Task DeleteSupplierAsync(int id);
}

public class SupplierService : ServiceBase, ISupplierService
{
    private const string _path = "Supplier";
    private readonly IRestProvider _restProvider;

    public SupplierService(IRestProvider restProvider)
    {
        _restProvider = restProvider;
    }

    public async Task<IEnumerable<SupplierDTO>> GetSuppliersAsync()
    {
        var response =
            await _restProvider.GetAsync(
                SetPathUrl(_path),
                id: null);

        var suppliers =
            await JsonProvider.DeserializeAsync<IEnumerable<SupplierDTO>>(response);

        return suppliers;
    }

    public async Task<SupplierDTO> GetSupplierAsync(int id)
    {
        var response =
            await _restProvider.GetAsync(
                SetPathUrl(_path + "/" + id),
                id: null);

        var supplier =
            await JsonProvider.DeserializeAsync<SupplierDTO>(response);

        return supplier;
    }

    public async Task SaveSupplierAsync(SupplierDTO supplierDTO)
    {
        PAW.Models.Supplier supplier =
            SupplierDTO.ConvertTo(supplierDTO);

        var suppliers = new List<PAW.Models.Supplier>
        {
            supplier
        };

        var content =
            JsonProvider.Serialize(suppliers);

        await _restProvider.PostAsync(
            SetPathUrl(_path),
            content);
    }

    public async Task DeleteSupplierAsync(int id)
    {
        await _restProvider.DeleteAsync(
            SetPathUrl(_path + "/" + id),
            string.Empty);
    }
}