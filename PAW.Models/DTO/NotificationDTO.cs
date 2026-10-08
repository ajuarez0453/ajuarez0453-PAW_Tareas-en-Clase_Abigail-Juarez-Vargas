using System.Text.Json.Serialization;

namespace PAW.Models.DTO;

public class NotificationDTO
{
	[JsonPropertyName("id")]
	public int Id { get; set; }

	[JsonPropertyName("userId")]
	public int UserId { get; set; }

	[JsonPropertyName("message")]
	public string? Message { get; set; }

	[JsonPropertyName("isRead")]
	public bool? IsRead { get; set; }

	[JsonPropertyName("createdAt")]
	public DateTime? CreatedAt { get; set; }


	public static NotificationDTO ConvertFrom(Notification notification)
	{
		return new NotificationDTO
		{
			Id = notification.Id,
			UserId = notification.UserId,
			Message = notification.Message,
			IsRead = notification.IsRead,
			CreatedAt = notification.CreatedAt
		};
	}


	public static Notification ConvertTo(NotificationDTO dto)
	{
		return new Notification
		{
			Id = dto.Id,
			UserId = dto.UserId,
			Message = dto.Message ?? string.Empty,
			IsRead = dto.IsRead,
			CreatedAt = dto.CreatedAt
		};
	}
}