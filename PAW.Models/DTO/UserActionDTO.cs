using System.Text.Json.Serialization;

namespace PAW.Models.DTO;

public class UserActionDTO
{
	[JsonPropertyName("id")]
	public decimal? Id { get; set; }

	[JsonPropertyName("name")]
	public string? Name { get; set; }

	[JsonPropertyName("description")]
	public string? Description { get; set; }


	public static UserActionDTO ConvertFrom(UserAction action)
	{
		return new UserActionDTO
		{
			Id = action.Id,
			Name = action.Name,
			Description = action.Description
		};
	}


	public static UserAction ConvertTo(UserActionDTO dto)
	{
		return new UserAction
		{
			Id = dto.Id,
			Name = dto.Name,
			Description = dto.Description
		};
	}
}