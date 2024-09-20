using Fantasy.Backend.Repositories.Interfaces;
using Fantasy.Backend.UnitsOfWork.interfaces;
using Fantasy.shared.Entities;
using Fantasy.shared.Responses;

namespace Fantasy.Backend.UnitsOfWork.Implementations;

public class CountriesUnitOfWork : GenericUnitOfWork<Country>, ICountriesUnitOfWork
{
    private readonly ICountriesRepository _countriesRepository;

    public CountriesUnitOfWork(IGenericRepository<Country> repository, ICountriesRepository countriesRepository) : base(repository)
    {
        _countriesRepository = countriesRepository;
    }

    public override async Task<ActionsResponse<IEnumerable<Country>>> GetAsync() => await _countriesRepository.GetAsync();

    public override async Task<ActionsResponse<Country>> GetAsync(int id) => await _countriesRepository.GetAsync(id);

    public async Task<IEnumerable<Country>> GetComboAsync() => await _countriesRepository.GetComboAsync();
}