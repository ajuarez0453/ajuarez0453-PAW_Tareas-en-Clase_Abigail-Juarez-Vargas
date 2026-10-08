using PAW.Models;
using PAW.Repositories;

namespace PAW.DataAccess.Repositories;

public interface IUserRepository
	: IRepositoryBase<User>
{
}

public class UserRepository
	: RepositoryBase<User>,
	  IUserRepository
{
}