using PAW.Models;
using PAW.Repositories;

namespace PAW.DataAccess.Repositories;

public interface IRoleRepository
	: IRepositoryBase<Role>
{
}

public class RoleRepository
	: RepositoryBase<Role>,
	  IRoleRepository
{
}