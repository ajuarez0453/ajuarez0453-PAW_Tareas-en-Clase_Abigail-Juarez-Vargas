using System.Text.Json.Serialization;

namespace PAW.Models.DTO;

public class ProductDTO
{
	[JsonPropertyName("productId")]
	public int ProductId { get; set; }

	[JsonPropertyName("name")]
	public string? Name { get; set; }

	[JsonPropertyName("description")]
	public string? Description { get; set; }

	[JsonPropertyName("rating")]
	public decimal? Rating { get; set; }

	[JsonPropertyName("inventoryId")]
	public int? InventoryId { get; set; }

	[JsonPropertyName("supplierId")]
	public int? SupplierId { get; set; }

	[JsonPropertyName("categoryId")]
	public int? CategoryId { get; set; }

	[JsonPropertyName("modifiedBy")]
	public string? ModifiedBy { get; set; }

	[JsonPropertyName("createdBy")]
	public string? CreatedBy { get; set; }

	[JsonPropertyName("lastModified")]
	public DateTime? LastModified { get; set; }

	public static ProductDTO ConvertFrom(Product product)
	{
		return new ProductDTO
		{
			ProductId = product.ProductId,
			Name = product.ProductName,
			Description = product.Description,
			Rating = product.Rating,
			InventoryId = product.InventoryId,
			SupplierId = product.SupplierId,
			CategoryId = product.CategoryId,
			ModifiedBy = product.ModifiedBy,
			CreatedBy = product.CreatedBy,
			LastModified = product.LastModified
		};
	}

	public static Product ConvertTo(ProductDTO dto)
	{
		return new Product
		{
			ProductId = dto.ProductId,
			ProductName = dto.Name,
			Description = dto.Description,
			Rating = dto.Rating,
			InventoryId = dto.InventoryId,
			SupplierId = dto.SupplierId,
			CategoryId = dto.CategoryId,
			ModifiedBy = dto.ModifiedBy,
			CreatedBy = dto.CreatedBy,
			LastModified = dto.LastModified
		};
	}
}