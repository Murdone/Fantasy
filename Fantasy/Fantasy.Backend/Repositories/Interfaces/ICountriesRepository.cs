using Fantasy.shared.Entities;
using Fantasy.shared.Responses;

namespace Fantasy.Backend.Repositories.Interfaces;

public interface ICountriesRepository
{
    Task<ActionsResponse<Country>> GetAsync(int id);

    Task<ActionsResponse<IEnumerable<Country>>> GetAsync();

    Task<IEnumerable<Country>> GetComboAsync();
}