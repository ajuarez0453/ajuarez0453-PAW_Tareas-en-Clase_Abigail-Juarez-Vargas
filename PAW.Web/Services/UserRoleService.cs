using APW.Architecture;
using PAW.Architecture.Providers;
using PAW.Models.DTO;

namespace PAW.Web.Services;

public interface IUserRoleService
{
	Task<IEnumerable<UserRoleDTO>> GetUserRolesAsync();
	Task<UserRoleDTO?> GetUserRoleAsync(decimal id);
}

public class UserRoleService
	: ServiceBase,
	  IUserRoleService
{
	private const string _path = "UserRole";

	private readonly IRestProvider _restProvider;

	public UserRoleService(
		IRestProvider restProvider)
	{
		_restProvider = restProvider;
	}

	public async Task<IEnumerable<UserRoleDTO>> GetUserRolesAsync()
	{
		var response =
			await _restProvider.GetAsync(
				SetPathUrl(_path),
				null);

		var userRoles =
			await JsonProvider.DeserializeAsync<
				IEnumerable<UserRoleDTO>
			>(response);

		return userRoles ?? [];
	}

	public async Task<UserRoleDTO?> GetUserRoleAsync(decimal id)
	{
		var response =
			await _restProvider.GetAsync(
				SetPathUrl(_path),
				id.ToString());

		return await JsonProvider.DeserializeAsync<UserRoleDTO>(
			response);
	}
}