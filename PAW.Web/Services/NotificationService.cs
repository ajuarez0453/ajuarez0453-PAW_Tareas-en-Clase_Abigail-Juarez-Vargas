using APW.Architecture;
using PAW.Architecture.Providers;
using PAW.Models.DTO;

namespace PAW.Web.Services;

public interface INotificationService
{
	Task<IEnumerable<NotificationDTO>> GetNotificationsAsync();

	Task<NotificationDTO?> GetNotificationAsync(int id);

	Task CreateNotificationAsync(NotificationDTO notification);

	Task UpdateNotificationAsync(NotificationDTO notification);

	Task DeleteNotificationAsync(int id);
}


public class NotificationService
	: ServiceBase,
	  INotificationService
{
	private const string _path = "Notification";

	private readonly IRestProvider _restProvider;


	public NotificationService(
		IRestProvider restProvider)
	{
		_restProvider = restProvider;
	}


	public async Task<IEnumerable<NotificationDTO>>
		GetNotificationsAsync()
	{
		var response =
			await _restProvider.GetAsync(
				SetPathUrl(_path),
				null);

		var notifications =
			await JsonProvider.DeserializeAsync<
				IEnumerable<NotificationDTO>
			>(response);

		return notifications ?? [];
	}


	public async Task<NotificationDTO?>
		GetNotificationAsync(int id)
	{
		var response =
			await _restProvider.GetAsync(
				SetPathUrl(_path),
				id.ToString());

		return await JsonProvider.DeserializeAsync<
			NotificationDTO
		>(response);
	}


	public async Task CreateNotificationAsync(
		NotificationDTO notification)
	{
		var json =
			JsonProvider.Serialize(notification);

		await _restProvider.PostAsync(
			SetPathUrl(_path),
			json);
	}


	public async Task UpdateNotificationAsync(
		NotificationDTO notification)
	{
		var json =
			JsonProvider.Serialize(notification);

		await _restProvider.PutAsync(
			SetPathUrl(_path),
			notification.Id.ToString(),
			json);
	}


	public async Task DeleteNotificationAsync(int id)
	{
		await _restProvider.DeleteAsync(
			SetPathUrl(_path),
			id.ToString());
	}
}