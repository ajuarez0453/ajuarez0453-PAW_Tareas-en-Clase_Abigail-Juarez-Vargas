using PAW.Models;
using PAW.Repositories;

namespace PAW.DataAccess.Repositories;

public interface IInventoryRepository
	: IRepositoryBase<Inventory>
{
}

public class InventoryRepository
	: RepositoryBase<Inventory>,
	  IInventoryRepository
{
}