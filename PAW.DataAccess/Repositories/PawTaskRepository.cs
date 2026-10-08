using PAW.Repositories;
using PawTask = PAW.Models.Task;

namespace PAW.DataAccess.Repositories;

public interface IPawTaskRepository
	: IRepositoryBase<PawTask>
{
}

public class PawTaskRepository
	: RepositoryBase<PawTask>,
	  IPawTaskRepository
{
}