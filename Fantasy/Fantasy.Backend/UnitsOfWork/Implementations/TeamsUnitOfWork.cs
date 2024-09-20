using Fantasy.Backend.Repositories.Interfaces;
using Fantasy.Backend.UnitsOfWork.interfaces;
using Fantasy.Frontend.DTOs;
using Fantasy.shared.Entities;
using Fantasy.shared.Responses;

namespace Fantasy.Backend.UnitsOfWork.Implementations;

public class TeamsUnitOfWork : GenericUnitOfWork<Team>, ITeamsUnitOfWork
{
    private readonly ITeamsRepository _teamsRepository;

    public TeamsUnitOfWork(IGenericRepository<Team> repository, ITeamsRepository teamsRepository) : base(repository)
    {
        _teamsRepository = teamsRepository;
    }

    public async Task<ActionsResponse<Team>> AddAsync(TeamDTO teamDTO) => await _teamsRepository.AddAsync(teamDTO);

    public async Task<IEnumerable<Team>> GetComboAsync(int countryId) => await _teamsRepository.GetComboAsync(countryId);

    public async Task<ActionsResponse<Team>> UpdateAsync(TeamDTO teamDTO) => await _teamsRepository.UpdateAsync(teamDTO);

    public override async Task<ActionsResponse<Team>> GetAsync(int id) => await _teamsRepository.GetAsync(id);

    public override async Task<ActionsResponse<IEnumerable<Team>>> GetAsync() => await _teamsRepository.GetAsync();
}