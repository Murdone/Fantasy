using Fantasy.Frontend.DTOs;
using Fantasy.shared.Entities;
using Fantasy.shared.Responses;

namespace Fantasy.Backend.UnitsOfWork.interfaces;

public interface ITeamsUnitOfWork
{
    Task<IEnumerable<Team>> GetComboAsync(int countryId);

    Task<ActionsResponse<Team>> AddAsync(TeamDTO teamDTO);

    Task<ActionsResponse<Team>> UpdateAsync(TeamDTO teamDTO);

    Task<ActionsResponse<Team>> GetAsync(int id);

    Task<ActionsResponse<IEnumerable<Team>>> GetAsync();
}