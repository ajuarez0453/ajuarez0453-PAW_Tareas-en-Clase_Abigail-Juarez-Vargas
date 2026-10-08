using APW.Architecture;
using PAW.Architecture.Providers;
using PAW.Models.DTO;

namespace PAW.Web.Services;

public interface IProductService
{
	Task<IEnumerable<ProductDTO>> GetProductsAsync();

	Task<ProductDTO?> GetProductAsync(int id);

	Task CreateProductAsync(ProductDTO product);

	Task UpdateProductAsync(ProductDTO product);

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
			await _restProvider.GetAsync(
				SetPathUrl(_path),
				id: null);

		var products =
			await JsonProvider.DeserializeAsync<
				IEnumerable<ProductDTO>
			>(response);

		return products ?? [];
	}


	public async Task<ProductDTO?> GetProductAsync(int id)
	{
		var response =
			await _restProvider.GetAsync(
				SetPathUrl(_path),
				id.ToString());

		return await JsonProvider.DeserializeAsync<ProductDTO>(
			response);
	}


	public async Task CreateProductAsync(ProductDTO product)
	{
		var json =
			JsonProvider.Serialize(product);

		await _restProvider.PostAsync(
			SetPathUrl(_path),
			json);
	}


	public async Task UpdateProductAsync(ProductDTO product)
	{
		var json =
			JsonProvider.Serialize(product);

		await _restProvider.PutAsync(
			SetPathUrl(_path),
			product.ProductId.ToString(),
			json);
	}


	public async Task DeleteProductAsync(int id)
	{
		await _restProvider.DeleteAsync(
			SetPathUrl(_path),
			id.ToString());
	}
}