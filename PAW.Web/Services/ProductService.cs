using APW.Architecture;
using PAW.Architecture.Providers;
using PAW.Models.DTO;

namespace PAW.Web.Services;

public interface IProductService
{
    Task<IEnumerable<ProductDTO>> GetProductsAsync();
    Task<ProductDTO> GetProductAsync(int id);
    Task SaveProductAsync(ProductDTO productDTO);
    Task DeleteProductAsync(int id);
}

public class ProductService : ServiceBase, IProductService
{
    private const string _path = "Product";
    private readonly IRestProvider _restProvider;

    public ProductService(IRestProvider restProvider)
    {
        _restProvider = restProvider;
    }

    public async Task<IEnumerable<ProductDTO>> GetProductsAsync()
    {
        var response =
            await _restProvider.GetAsync(SetPathUrl(_path), id: null);

        var products =
            await JsonProvider.DeserializeAsync<IEnumerable<ProductDTO>>(response);

        return products;
    }

    public async Task<ProductDTO> GetProductAsync(int id)
    {
        var response =
            await _restProvider.GetAsync(SetPathUrl(_path + "/" + id), id: null);

        var product =
            await JsonProvider.DeserializeAsync<ProductDTO>(response);

        return product;
    }

    public async Task SaveProductAsync(ProductDTO productDTO)
    {
        PAW.Models.Product product = ProductDTO.ConvertTo(productDTO);

        var products = new List<PAW.Models.Product>
        {
            product
        };

        var content = JsonProvider.Serialize(products);

        await _restProvider.PostAsync(SetPathUrl(_path), content);
    }
    public async Task DeleteProductAsync(int id)
    {
        await _restProvider.DeleteAsync(
            SetPathUrl(_path + "/" + id),
            string.Empty
        );
    }

}