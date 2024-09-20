using Fantasy.shared.Entities;
using Fantasy.shared.Responses;

namespace Fantasy.Backend.UnitsOfWork.interfaces;

public interface ICountriesUnitOfWork
{
    Task<ActionsResponse<Country>> GetAsync(int id);

    Task<ActionsResponse<IEnumerable<Country>>> GetAsync();

    Task<IEnumerable<Country>> GetComboAsync();
}