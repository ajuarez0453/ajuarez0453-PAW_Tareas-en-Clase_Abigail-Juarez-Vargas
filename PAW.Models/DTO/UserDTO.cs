using System.Text.Json.Serialization;

namespace PAW.Models.DTO;

public class UserDTO
{
	[JsonPropertyName("userId")]
	public int UserId { get; set; }

	[JsonPropertyName("username")]
	public string? Username { get; set; }

	[JsonPropertyName("email")]
	public string? Email { get; set; }

	[JsonPropertyName("passwordHash")]
	public string? PasswordHash { get; set; }

	[JsonPropertyName("createdAt")]
	public DateTime? CreatedAt { get; set; }

	[JsonPropertyName("isActive")]
	public bool? IsActive { get; set; }

	[JsonPropertyName("modifiedBy")]
	public string? ModifiedBy { get; set; }

	[JsonPropertyName("roleId")]
	public int? RoleId { get; set; }

	[JsonPropertyName("lastModifiedBy")]
	public string? LastModifiedBy { get; set; }

	public static UserDTO ConvertFrom(User user)
	{
		return new UserDTO
		{
			UserId = user.UserId,
			Username = user.Username,
			Email = user.Email,
			PasswordHash = user.PasswordHash,
			CreatedAt = user.CreatedAt,
			IsActive = user.IsActive,
			RoleId = user.RoleId,
			LastModifiedBy = user.LastModifiedBy
		};
	}

	public static User ConvertTo(UserDTO dto)
	{
		return new User
		{
			UserId = dto.UserId,
			Username = dto.Username,
			Email = dto.Email,
			PasswordHash = dto.PasswordHash,
			CreatedAt = dto.CreatedAt,
			IsActive = dto.IsActive,
			ModifiedBy = dto.ModifiedBy,
			RoleId = dto.RoleId,
			LastModifiedBy = dto.LastModifiedBy
		};
	}
}