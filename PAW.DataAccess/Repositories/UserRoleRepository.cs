using Microsoft.EntityFrameworkCore;
using PAW.DataAccess.MSSQL;
using PAW.Models;

namespace PAW.DataAccess.Repositories;

public interface IUserRoleRepository
{
	Task<IEnumerable<UserRole>> ReadAsync();
}

public class UserRoleRepository : IUserRoleRepository
{
	private readonly ProductDbContext _context;

	public UserRoleRepository()
	{
		_context = new ProductDbContext();
	}

	public async Task<IEnumerable<UserRole>> ReadAsync()
	{
		return await _context.UserRoles
			.AsNoTracking()
			.ToListAsync();
	}
}