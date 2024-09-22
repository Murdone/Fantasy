using Fantasy.Backend.Data;
using Fantasy.Backend.Helpers;
using Fantasy.Backend.Repositories.Interfaces;
using Fantasy.shared.DTOs;
using Fantasy.shared.Responses;
using Microsoft.EntityFrameworkCore;

namespace Fantasy.Backend.Repositories.Implementations;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly DataContext _context;
    private readonly DbSet<T> _entity;

    public GenericRepository(DataContext context)
    {
        _context = context;
        _entity = context.Set<T>();
    }

    public virtual async Task<ActionsResponse<T>> AddAsync(T entity)
    {
        _context.Add(entity);
        try
        {
            await _context.SaveChangesAsync();
            return new ActionsResponse<T>
            {
                WasSuccess = true,
                Result = entity
            };
        }
        catch (DbUpdateException)
        {
            return DbUpdateExceptionActionResponse();
        }
        catch (Exception exception)
        {
            return ExceptionActionResponse(exception);
        }
    }

    public virtual async Task<ActionsResponse<T>> DeleteAsync(int id)
    {
        var row = await _entity.FindAsync(id);
        if (row == null)
        {
            return new ActionsResponse<T>
            {
                WasSuccess = false,
                Message = "ERR001"
            };
        }
        _entity.Remove(row);
        try
        {
            await _context.SaveChangesAsync();
            return new ActionsResponse<T>
            {
                WasSuccess = true,
            };
        }
        catch
        {
            return new ActionsResponse<T>
            {
                WasSuccess = false,
                Message = "ERR002"
            };
        }
    }

    public virtual async Task<ActionsResponse<T>> GetAsync(int id)
    {
        var row = await _entity.FindAsync(id);
        if (row == null)
        {
            return new ActionsResponse<T>
            {
                WasSuccess = false,
                Message = "ERR001"
            };
        }
        return new ActionsResponse<T>
        {
            WasSuccess = true,
            Result = row
        };
    }

    public virtual async Task<ActionsResponse<IEnumerable<T>>> GetAsync()
    {
        return new ActionsResponse<IEnumerable<T>>
        {
            WasSuccess = true,
            Result = await _entity.ToListAsync()
        };
    }

    public virtual async Task<ActionsResponse<T>> UpdateAsync(T entity)
    {
        _context.Update(entity);
        try
        {
            await _context.SaveChangesAsync();
            return new ActionsResponse<T>
            {
                WasSuccess = true,
                Result = entity
            };
        }
        catch (DbUpdateException)
        {
            return DbUpdateExceptionActionResponse();
        }
        catch (Exception exception)
        {
            return ExceptionActionResponse(exception);
        }
    }

    private ActionsResponse<T> ExceptionActionResponse(Exception exception)
    {
        return new ActionsResponse<T>
        {
            WasSuccess = false,
            Message = exception.Message
        };
    }

    private ActionsResponse<T> DbUpdateExceptionActionResponse()
    {
        return new ActionsResponse<T>
        {
            WasSuccess = false,
            Message = "ERR003"
        };
    }

    public virtual async Task<ActionsResponse<IEnumerable<T>>> GetAsync(PaginationDTO pagination)
    {
        var queryable = _entity.AsQueryable();

        return new ActionsResponse<IEnumerable<T>>
        {
            WasSuccess = true,
            Result = await queryable
                .Paginate(pagination)
                .ToListAsync()
        };
    }

    public virtual async Task<ActionsResponse<int>> GetTotalRecordsAsync()
    {
        var queryable = _entity.AsQueryable();
        double count = await queryable.CountAsync();
        return new ActionsResponse<int>
        {
            WasSuccess = true,
            Result = (int)count
        };
    }
}