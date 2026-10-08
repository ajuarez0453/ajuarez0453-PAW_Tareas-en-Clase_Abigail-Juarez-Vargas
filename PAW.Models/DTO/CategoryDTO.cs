using System.Text.Json.Serialization;

namespace PAW.Models.DTO;

public class CategoryDTO
{
	[JsonPropertyName("categoryId")]
	public int CategoryId { get; set; }

	[JsonPropertyName("categoryName")]
	public string? CategoryName { get; set; }

	[JsonPropertyName("description")]
	public string? Description { get; set; }

	[JsonPropertyName("lastModified")]
	public DateTime? LastModified { get; set; }

	[JsonPropertyName("modifiedBy")]
	public string? ModifiedBy { get; set; }


	public static CategoryDTO ConvertFrom(Category category)
	{
		return new CategoryDTO
		{
			CategoryId = category.CategoryId,
			CategoryName = category.CategoryName,
			Description = category.Description,
			LastModified = category.LastModified,
			ModifiedBy = category.ModifiedBy
		};
	}


	public static Category ConvertTo(CategoryDTO dto)
	{
		return new Category
		{
			CategoryId = dto.CategoryId,
			CategoryName = dto.CategoryName,
			Description = dto.Description,
			LastModified = dto.LastModified,
			ModifiedBy = dto.ModifiedBy
		};
	}
}