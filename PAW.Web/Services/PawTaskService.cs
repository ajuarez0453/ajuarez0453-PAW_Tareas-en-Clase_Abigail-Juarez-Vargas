using APW.Architecture;
using PAW.Architecture.Providers;
using PAW.Models.DTO;

namespace PAW.Web.Services;

public interface IPawTaskService
{
	Task<IEnumerable<PawTaskDTO>> GetTasksAsync();

	Task<PawTaskDTO?> GetTaskAsync(int id);

	Task CreateTaskAsync(PawTaskDTO task);

	Task UpdateTaskAsync(PawTaskDTO task);

	Task DeleteTaskAsync(int id);
}


public class PawTaskService
	: ServiceBase,
	  IPawTaskService
{
	private const string _path = "PawTask";

	private readonly IRestProvider _restProvider;


	public PawTaskService(
		IRestProvider restProvider)
	{
		_restProvider = restProvider;
	}


	public async Task<IEnumerable<PawTaskDTO>> GetTasksAsync()
	{
		var response =
			await _restProvider.GetAsync(
				SetPathUrl(_path),
				null);

		var tasks =
			await JsonProvider.DeserializeAsync<
				IEnumerable<PawTaskDTO>
			>(response);

		return tasks ?? [];
	}


	public async Task<PawTaskDTO?> GetTaskAsync(int id)
	{
		var response =
			await _restProvider.GetAsync(
				SetPathUrl(_path),
				id.ToString());

		return await JsonProvider.DeserializeAsync<PawTaskDTO>(
			response);
	}


	public async Task CreateTaskAsync(PawTaskDTO task)
	{
		var json =
			JsonProvider.Serialize(task);

		await _restProvider.PostAsync(
			SetPathUrl(_path),
			json);
	}


	public async Task UpdateTaskAsync(PawTaskDTO task)
	{
		var json =
			JsonProvider.Serialize(task);

		await _restProvider.PutAsync(
			SetPathUrl(_path),
			task.Id.ToString(),
			json);
	}


	public async Task DeleteTaskAsync(int id)
	{
		await _restProvider.DeleteAsync(
			SetPathUrl(_path),
			id.ToString());
	}
}