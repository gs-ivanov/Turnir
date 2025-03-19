namespace Turnir.Services.Teams
{
    using System.Collections.Generic;
    using System.Linq;
    using Turnir.Data;
    using Turnir.Models;

    public class TeamService : ITeamService
    {
        private readonly TurnirDbContext data;

        public TeamService(TurnirDbContext data)
            => this.data = data;

        public TeamQueryServiceModel All(
            string name,
            string searchTerm,
            TeamSorting sorting,
            int currentPage,
            int teamsPerPage)
        {
            var teamsQuery = this.data.Teams.AsQueryable();

            if (!string.IsNullOrWhiteSpace(name))
            {
                teamsQuery = teamsQuery.Where(c => c.Name == name);
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                teamsQuery = teamsQuery.Where(c =>
                    (c.Name + " " + c.City).ToLower().Contains(searchTerm.ToLower()) ||
                    c.Description.ToLower().Contains(searchTerm.ToLower()));
            }

            teamsQuery = sorting switch
            {
                TeamSorting.Year => teamsQuery.OrderByDescending(c => c.Year),
                TeamSorting.NameAndCity => teamsQuery.OrderBy(c => c.Name).ThenBy(c => c.City),
                TeamSorting.DateCreated or _ => teamsQuery.OrderByDescending(c => c.Id)
            };

            var totalTeams = teamsQuery.Count();

            var teams = teamsQuery
                .Skip((currentPage - 1) * teamsPerPage)
                .Take(teamsPerPage)
                .Select(c => new TeamServiceModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    City = c.City,
                    Year = c.Year,
                    TeamLogo = c.TeamLogo,
                    Group = c.Group.Name
                })
                .ToList();

            return new TeamQueryServiceModel
            {
                TotalTeams = totalTeams,
                CurrentPage = currentPage,
                TeamsPerPage = teamsPerPage,
                Teams = teams
            };
        }

        public IEnumerable<string> AllTeamNames()
            => this.data
                .Teams
                .Select(c => c.Name)
                .Distinct()
                .OrderBy(nm => nm)
                .ToList();
    }
}
