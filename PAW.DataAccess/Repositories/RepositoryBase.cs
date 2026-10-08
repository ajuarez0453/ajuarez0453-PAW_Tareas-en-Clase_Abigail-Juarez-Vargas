using Microsoft.EntityFrameworkCore;
using PAW.DataAccess.MSSQL;

namespace PAW.Repositories;

/// <summary>
/// Interface for basic repository operations.
/// </summary>
/// <typeparam name="T">The type of entity.</typeparam>
public interface IRepositoryBase<T>
{
	Task<bool> UpsertAsync(T entity, bool isUpdating);

	Task<bool> CreateAsync(T entity);

	Task<bool> DeleteAsync(T entity);

	Task<IEnumerable<T>> ReadAsync();

	Task<T> FindAsync(int id);

	Task<bool> UpdateAsync(T entity);

	Task<bool> UpdateManyAsync(IEnumerable<T> entities);

	Task<bool> ExistsAsync(T entity);
}


/// <summary>
/// Base class for repository operations.
/// </summary>
/// <typeparam name="T">Entity type.</typeparam>
public abstract class RepositoryBase<T>
	: IRepositoryBase<T>
	where T : class
{
	private readonly ProductDbContext _context;

	protected ProductDbContext DbContext => _context;

	protected DbSet<T> DbSet;


	public RepositoryBase()
	{
		_context = new ProductDbContext();

		DbSet = _context.Set<T>();
	}


	public async Task<bool> UpsertAsync(
		T entity,
		bool isUpdating)
	{
		return isUpdating
			? await UpdateAsync(entity)
			: await CreateAsync(entity);
	}


	public async Task<bool> CreateAsync(T entity)
	{
		try
		{
			await _context.AddAsync(entity);

			return await SaveAsync();
		}
		catch (Exception ex)
		{
			throw new PAWException(ex);
		}
	}


	public async Task<bool> UpdateAsync(T entity)
	{
		try
		{
			_context.Update(entity);

			return await SaveAsync();
		}
		catch (Exception ex)
		{
			throw new PAWException(ex);
		}
	}


	public async Task<bool> UpdateManyAsync(
		IEnumerable<T> entities)
	{
		try
		{
			_context.UpdateRange(entities);

			return await SaveAsync();
		}
		catch (Exception ex)
		{
			throw new PAWException(ex);
		}
	}


	public async Task<bool> DeleteAsync(T entity)
	{
		try
		{
			_context.Remove(entity);

			return await SaveAsync();
		}
		catch (Exception ex)
		{
			throw new PAWException(ex);
		}
	}

	public async Task<IEnumerable<T>> ReadAsync()
	{
		try
		{
			return await _context
				.Set<T>()
				.ToListAsync();
		}
		catch (Exception ex)
		{
			throw new Exception(
				$"Error leyendo {typeof(T).Name}: {ex.Message}",
				ex);
		}
	}


	public async Task<T> FindAsync(int id)
	{
		try
		{
			return await _context
				.Set<T>()
				.FindAsync(id);
		}
		catch (Exception ex)
		{
			throw new PAWException(ex);
		}
	}


	public async Task<bool> ExistsAsync(T entity)
	{
		try
		{
			var items =
				await ReadAsync();

			return items.Any(
				x => x.Equals(entity));
		}
		catch (Exception ex)
		{
			throw new PAWException(ex);
		}
	}


	protected async Task<bool> SaveAsync()
	{
		var result =
			await _context.SaveChangesAsync();

		return result > 0;
	}


	public virtual async Task<object> GetData()
	{
		await System.Threading.Tasks.Task.Run(
			() => { });

		return null;
	}
}