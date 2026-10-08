using APW.Architecture;
using PAW.Architecture.Providers;
using PAW.Models.DTO;

namespace PAW.Web.Services;

public interface IUserService
{
	Task<IEnumerable<UserDTO>> GetUsersAsync();

	Task<UserDTO?> GetUserAsync(int id);

	Task CreateUserAsync(UserDTO user);

	Task UpdateUserAsync(UserDTO user);

	Task DeleteUserAsync(int id);
}

public class UserService : ServiceBase, IUserService
{
	private const string _path = "User";

	private readonly IRestProvider _restProvider;

	public UserService(IRestProvider restProvider)
	{
		_restProvider = restProvider;
	}

	public async Task<IEnumerable<UserDTO>> GetUsersAsync()
	{
		var response =
			await _restProvider.GetAsync(
				SetPathUrl(_path),
				null);

		var users =
			await JsonProvider.DeserializeAsync<
				IEnumerable<UserDTO>
			>(response);

		return users ?? [];
	}

	public async Task<UserDTO?> GetUserAsync(int id)
	{
		var response =
			await _restProvider.GetAsync(
				SetPathUrl(_path),
				id.ToString());

		return await JsonProvider.DeserializeAsync<UserDTO>(
			response);
	}

	public async Task CreateUserAsync(UserDTO user)
	{
		var json =
			JsonProvider.Serialize(user);

		await _restProvider.PostAsync(
			SetPathUrl(_path),
			json);
	}

	public async Task UpdateUserAsync(UserDTO user)
	{
		var json =
			JsonProvider.Serialize(user);

		await _restProvider.PutAsync(
			SetPathUrl(_path),
			user.UserId.ToString(),
			json);
	}

	public async Task DeleteUserAsync(int id)
	{
		await _restProvider.DeleteAsync(
			SetPathUrl(_path),
			id.ToString());
	}
}