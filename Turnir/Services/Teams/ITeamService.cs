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

        TeamDetailsServiceModel Details(int teamId);

        int Create(
            string name,
            string city,
            string description,
            string teamLogo,
            int year,
            int groupId,
            int trenerId);

        bool Edit(
            int carId,
            string name,
            string city,
            string description,
            string teamLogo,
            int year,
            int groupId);

        public IEnumerable<TeamServiceModel> IsByUser(string userId);

        public bool IsByTrener(int teamId, int trenerId);

        IEnumerable<string> AllTeamNames();

        IEnumerable<TeamGroupServiceModel> AllGroups();

        bool GroupExists(int groupId);
    }
}
