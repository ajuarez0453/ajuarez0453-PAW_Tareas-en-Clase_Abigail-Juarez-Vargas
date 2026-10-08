using Microsoft.EntityFrameworkCore;
using PAW.DataAccess.MSSQL;
using PAW.Models;

namespace PAW.DataAccess.Repositories;

public interface IUserActionRepository
{
	Task<IEnumerable<UserAction>> ReadAsync();
}

public class UserActionRepository : IUserActionRepository
{
	private readonly ProductDbContext _context;

	public UserActionRepository()
	{
		_context = new ProductDbContext();
	}

	public async Task<IEnumerable<UserAction>> ReadAsync()
	{
		return await _context.UserActions
			.AsNoTracking()
			.ToListAsync();
	}
}