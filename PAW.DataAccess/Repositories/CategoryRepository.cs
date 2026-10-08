using PAW.Models;

namespace PAW.DataAccess.Repositories;

public interface ICategoryRepository
	: PAW.Repositories.IRepositoryBase<Category>
{
}

public class CategoryRepository
	: PAW.Repositories.RepositoryBase<Category>,
	  ICategoryRepository
{
}