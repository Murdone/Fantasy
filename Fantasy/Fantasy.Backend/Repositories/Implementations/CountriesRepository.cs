using Fantasy.Backend.Data;
using Fantasy.Backend.Repositories.Interfaces;
using Fantasy.shared.Entities;
using Fantasy.shared.Responses;
using Microsoft.EntityFrameworkCore;

namespace Fantasy.Backend.Repositories.Implementations;

public class CountriesRepository : GenericRepository<Country>, ICountriesRepository
{
    private readonly DataContext _context;

    public CountriesRepository(DataContext context) : base(context)
    {
        _context = context;
    }

    public override async Task<ActionsResponse<IEnumerable<Country>>> GetAsync()
    {
        var countries = await _context.Countries
            .Include(c => c.Teams)
            .ToListAsync();
        return new ActionsResponse<IEnumerable<Country>>
        {
            WasSuccess = true,
            Result = countries
        };
    }

    public override async Task<ActionsResponse<Country>> GetAsync(int id)
    {
        var country = await _context.Countries
             .Include(c => c.Teams)
             .FirstOrDefaultAsync(c => c.Id == id);

        if (country == null)
        {
            return new ActionsResponse<Country>
            {
                WasSuccess = false,
                Message = "ERR001"
            };
        }

        return new ActionsResponse<Country>
        {
            WasSuccess = true,
            Result = country
        };
    }

    public async Task<IEnumerable<Country>> GetComboAsync()
    {
        return await _context.Countries
            .OrderBy(x => x.Name)
            .ToListAsync();
    }
}