using PAW.Models;
using PAW.Repositories;

namespace PAW.DataAccess.Repositories;

public interface IProductRepository : IRepositoryBase<Product>
{
}

public class ProductRepository
	: RepositoryBase<Product>, IProductRepository
{
}
