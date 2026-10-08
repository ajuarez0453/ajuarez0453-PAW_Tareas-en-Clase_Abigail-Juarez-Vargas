using APW.Architecture;
using PAW.Architecture.Providers;
using PAW.Models.DTO;

namespace PAW.Web.Services;

public interface ICategoryService
{
	Task<IEnumerable<CategoryDTO>> GetCategoriesAsync();

	Task<CategoryDTO?> GetCategoryAsync(int id);

	Task CreateCategoryAsync(CategoryDTO category);

	Task UpdateCategoryAsync(CategoryDTO category);

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
			await _restProvider.GetAsync(
				SetPathUrl(_path),
				null);

		var categories =
			await JsonProvider.DeserializeAsync<
				IEnumerable<CategoryDTO>
			>(response);

		return categories ?? [];
	}


	public async Task<CategoryDTO?> GetCategoryAsync(int id)
	{
		var response =
			await _restProvider.GetAsync(
				SetPathUrl(_path),
				id.ToString());

		return await JsonProvider.DeserializeAsync<CategoryDTO>(
			response);
	}


	public async Task CreateCategoryAsync(CategoryDTO category)
	{
		var json =
			JsonProvider.Serialize(category);

		await _restProvider.PostAsync(
			SetPathUrl(_path),
			json);
	}


	public async Task UpdateCategoryAsync(CategoryDTO category)
	{
		var json =
			JsonProvider.Serialize(category);

		await _restProvider.PutAsync(
			SetPathUrl(_path),
			category.CategoryId.ToString(),
			json);
	}


	public async Task DeleteCategoryAsync(int id)
	{
		await _restProvider.DeleteAsync(
			SetPathUrl(_path),
			id.ToString());
	}
}