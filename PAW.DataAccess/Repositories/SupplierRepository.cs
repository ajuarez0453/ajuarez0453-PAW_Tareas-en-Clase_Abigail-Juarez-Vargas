using PAW.Models;
using PAW.Repositories;

namespace PAW.DataAccess.Repositories;

public interface ISupplierRepository
	: IRepositoryBase<Supplier>
{
}

public class SupplierRepository
	: RepositoryBase<Supplier>,
	  ISupplierRepository
{
}