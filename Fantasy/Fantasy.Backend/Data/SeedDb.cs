using Fantasy.shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fantasy.Backend.Data;

public class SeedDb
{
    private readonly DataContext _context;

    public SeedDb(DataContext context)
    {
        _context = context;
    }

    public async Task SeedAsync()
    {
        await _context.Database.EnsureCreatedAsync();
        await CheckCountriesAsync();
        await CheckTeamsAsync();
    }

    private async Task CheckCountriesAsync()
    {
        if (!_context.Countries.Any())
        {
            var countriesSQLScript = File.ReadAllText("Data\\Countries.sql");
            await _context.Database.ExecuteSqlRawAsync(countriesSQLScript);
        }
    }

    private async Task CheckTeamsAsync()
    {
        if (!_context.Teams.Any())
        {
            foreach (var country in _context.Countries)
            {
                _context.Teams.Add(new Team { Name = country.Name, Country = country! });
                if (country.Name == "Chile")
                {
                    _context.Teams.Add(new Team { Name = "Colo colo", Country = country! });
                    _context.Teams.Add(new Team { Name = "U", Country = country! });
                    _context.Teams.Add(new Team { Name = "Universidad catolica", Country = country! });
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}