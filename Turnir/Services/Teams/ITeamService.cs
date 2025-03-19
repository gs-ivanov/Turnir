namespace Turnir.Services.Teams
{
    using System.Collections.Generic;
    using Turnir.Models;

    public interface ITeamService
    {
        TeamQueryServiceModel All(
            string name,
            string searchTerm,
            TeamSorting sorting,
            int currentPage,
            int teamsPerPage);

        IEnumerable<string> AllTeamNames();
    }
}
