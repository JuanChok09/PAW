using APW.Architecture;
using PAW.Architecture.Providers;
using PAW.Models.DTO;

namespace PAW.Web.Services;

public interface ICategoryService
{
    Task<IEnumerable<CategoryDTO>> GetCategoriesAsync();
    Task<CategoryDTO> GetCategoryAsync(int id);
    Task SaveCategoryAsync(CategoryDTO categoryDTO);
    Task DeleteCategoryAsync(int id);
}

public class CategoryService : ServiceBase, ICategoryService
{
    private const string _path = "Category";
    private readonly IRestProvider _restProvider;

    public CategoryService(IRestProvider restProvider)
    {
        _restProvider = restProvider;
    }

    public async Task<IEnumerable<CategoryDTO>> GetCategoriesAsync()
    {
        var response =
            await _restProvider.GetAsync(SetPathUrl(_path), id: null);

        var categories =
            await JsonProvider.DeserializeAsync<IEnumerable<CategoryDTO>>(response);

        return categories;
    }

    public async Task<CategoryDTO> GetCategoryAsync(int id)
    {
        var response =
            await _restProvider.GetAsync(SetPathUrl(_path + "/" + id), id: null);

        var category =
            await JsonProvider.DeserializeAsync<CategoryDTO>(response);

        return category;
    }

    public async Task SaveCategoryAsync(CategoryDTO categoryDTO)
    {
        PAW.Models.Category category =
            CategoryDTO.ConvertTo(categoryDTO);

        var categories = new List<PAW.Models.Category>
        {
            category
        };

        var content = JsonProvider.Serialize(categories);

        await _restProvider.PostAsync(SetPathUrl(_path), content);
    }

    public async Task DeleteCategoryAsync(int id)
    {
        await _restProvider.DeleteAsync(
            SetPathUrl(_path + "/" + id),
            string.Empty
        );
    }
}