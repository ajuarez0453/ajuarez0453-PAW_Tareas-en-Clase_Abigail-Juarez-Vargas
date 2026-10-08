using PAW.Models;
using PAW.Repositories;

namespace PAW.DataAccess.Repositories;

public interface INotificationRepository
	: IRepositoryBase<Notification>
{
}

public class NotificationRepository
	: RepositoryBase<Notification>,
	  INotificationRepository
{
}