using APW.Architecture;
using PAW.Architecture.Providers;
using PAW.Models.DTO;

namespace PAW.Web.Services;

public interface IComponentService
{
	Task<IEnumerable<ComponentDTO>> GetComponentsAsync();

	Task<ComponentDTO?> GetComponentAsync(decimal id);

	Task CreateComponentAsync(ComponentDTO component);

	Task UpdateComponentAsync(ComponentDTO component);

	Task DeleteComponentAsync(decimal id);
}


public class ComponentService : ServiceBase, IComponentService
{
	private const string _path = "Component";

	private readonly IRestProvider _restProvider;


	public ComponentService(IRestProvider restProvider)
	{
		_restProvider = restProvider;
	}


	public async Task<IEnumerable<ComponentDTO>> GetComponentsAsync()
	{
		var response =
			await _restProvider.GetAsync(
				SetPathUrl(_path),
				null);

		var components =
			await JsonProvider.DeserializeAsync<
				IEnumerable<ComponentDTO>
			>(response);

		return components ?? [];
	}


	public async Task<ComponentDTO?> GetComponentAsync(decimal id)
	{
		var response =
			await _restProvider.GetAsync(
				SetPathUrl(_path),
				id.ToString());

		return await JsonProvider.DeserializeAsync<ComponentDTO>(
			response);
	}


	public async Task CreateComponentAsync(ComponentDTO component)
	{
		var json =
			JsonProvider.Serialize(component);

		await _restProvider.PostAsync(
			SetPathUrl(_path),
			json);
	}


	public async Task UpdateComponentAsync(ComponentDTO component)
	{
		var json =
			JsonProvider.Serialize(component);

		await _restProvider.PutAsync(
			SetPathUrl(_path),
			component.Id.ToString(),
			json);
	}


	public async Task DeleteComponentAsync(decimal id)
	{
		await _restProvider.DeleteAsync(
			SetPathUrl(_path),
			id.ToString());
	}
}