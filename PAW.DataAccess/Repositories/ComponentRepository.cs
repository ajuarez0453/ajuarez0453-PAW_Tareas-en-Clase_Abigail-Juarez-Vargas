using PAW.Models;
using PAW.Repositories;

namespace PAW.DataAccess.Repositories;

public interface IComponentRepository
	: IRepositoryBase<Component>
{
}

public class ComponentRepository
	: RepositoryBase<Component>,
	  IComponentRepository
{
}