using APW.Architecture;
using PAW.Architecture.Providers;
using PAW.Models.DTO;

namespace PAW.Web.Services;

public interface ISupplierService
{
	Task<IEnumerable<SupplierDTO>> GetSuppliersAsync();

	Task<SupplierDTO?> GetSupplierAsync(int id);

	Task CreateSupplierAsync(SupplierDTO supplier);

	Task UpdateSupplierAsync(SupplierDTO supplier);

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
				null);

		var suppliers =
			await JsonProvider.DeserializeAsync<
				IEnumerable<SupplierDTO>
			>(response);

		return suppliers ?? [];
	}


	public async Task<SupplierDTO?> GetSupplierAsync(int id)
	{
		var response =
			await _restProvider.GetAsync(
				SetPathUrl(_path),
				id.ToString());

		return await JsonProvider.DeserializeAsync<SupplierDTO>(
			response);
	}


	public async Task CreateSupplierAsync(SupplierDTO supplier)
	{
		var json =
			JsonProvider.Serialize(supplier);

		await _restProvider.PostAsync(
			SetPathUrl(_path),
			json);
	}


	public async Task UpdateSupplierAsync(SupplierDTO supplier)
	{
		var json =
			JsonProvider.Serialize(supplier);

		await _restProvider.PutAsync(
			SetPathUrl(_path),
			supplier.SupplierId.ToString(),
			json);
	}


	public async Task DeleteSupplierAsync(int id)
	{
		await _restProvider.DeleteAsync(
			SetPathUrl(_path),
			id.ToString());
	}
}