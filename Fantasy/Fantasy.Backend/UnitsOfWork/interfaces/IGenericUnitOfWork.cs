using Fantasy.shared.Responses;

namespace Fantasy.Backend.UnitsOfWork.interfaces;

public interface IGenericUnitOfWork<T> where T : class
{
    Task<ActionsResponse<IEnumerable<T>>> GetAsync();

    Task<ActionsResponse<T>> AddAsync(T model);

    Task<ActionsResponse<T>> UpdateAsync(T model);

    Task<ActionsResponse<T>> DeleteAsync(int id);

    Task<ActionsResponse<T>> GetAsync(int id);
}