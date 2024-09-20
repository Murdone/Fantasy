using Fantasy.shared.Responses;

namespace Fantasy.Backend.Repositories.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<ActionsResponse<T>> GetAsync(int id);

        Task<ActionsResponse<IEnumerable<T>>> GetAsync();

        Task<ActionsResponse<T>> AddAsync(T entity);

        Task<ActionsResponse<T>> DeleteAsync(int id);

        Task<ActionsResponse<T>> UpdateAsync(T entity);
    }
}