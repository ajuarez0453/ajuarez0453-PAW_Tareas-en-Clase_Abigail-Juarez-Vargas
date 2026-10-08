using APW.Architecture;
using PAW.Architecture.Providers;
using PAW.Models.DTO;

namespace PAW.Web.Services;

public interface IRoleService
{
	Task<IEnumerable<RoleDTO>> GetRolesAsync();

	Task<RoleDTO?> GetRoleAsync(int id);

	Task CreateRoleAsync(RoleDTO role);

	Task UpdateRoleAsync(RoleDTO role);

	Task DeleteRoleAsync(int id);
}


public class RoleService : ServiceBase, IRoleService
{
	private const string _path = "Role";

	private readonly IRestProvider _restProvider;


	public RoleService(IRestProvider restProvider)
	{
		_restProvider = restProvider;
	}


	public async Task<IEnumerable<RoleDTO>> GetRolesAsync()
	{
		var response =
			await _restProvider.GetAsync(
				SetPathUrl(_path),
				null);

		var roles =
			await JsonProvider.DeserializeAsync<
				IEnumerable<RoleDTO>
			>(response);

		return roles ?? [];
	}


	public async Task<RoleDTO?> GetRoleAsync(int id)
	{
		var response =
			await _restProvider.GetAsync(
				SetPathUrl(_path),
				id.ToString());

		return await JsonProvider.DeserializeAsync<RoleDTO>(
			response);
	}


	public async Task CreateRoleAsync(RoleDTO role)
	{
		var json =
			JsonProvider.Serialize(role);

		await _restProvider.PostAsync(
			SetPathUrl(_path),
			json);
	}


	public async Task UpdateRoleAsync(RoleDTO role)
	{
		var json =
			JsonProvider.Serialize(role);

		await _restProvider.PutAsync(
			SetPathUrl(_path),
			role.RoleId.ToString(),
			json);
	}


	public async Task DeleteRoleAsync(int id)
	{
		await _restProvider.DeleteAsync(
			SetPathUrl(_path),
			id.ToString());
	}
}