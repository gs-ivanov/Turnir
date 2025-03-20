namespace Turnir.Services.Teams
{
    using System.Collections.Generic;
    using System.Linq;
    using Turnir.Data;
    using Turnir.Data.Models;
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

            var teams = GetTeams(teamsQuery)
                .Skip((currentPage - 1) * teamsPerPage)
                .Take(teamsPerPage);

            return new TeamQueryServiceModel
            {
                TotalTeams = totalTeams,
                CurrentPage = currentPage,
                TeamsPerPage = teamsPerPage,
                Teams = teams
            };
        }

        public TeamDetailsServiceModel Details(int id)
            => this.data
                .Teams
                .Where(c => c.Id == id)
                .Select(c => new TeamDetailsServiceModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    City = c.City,
                    Description = c.Description,
                    Year = c.Year,
                    TeamLogo = c.TeamLogo,
                    GroupName = c.Group.Name,
                    TrenerId = c.TrenerId,
                    TrenerName = c.Trener.Name,
                    UserId = c.Trener.UserId
                })
                .FirstOrDefault();

        public int Create(string name, string city, string description, string teamLogo, int year, int groupId, int trenerId)
        {
            var teamData = new Team
            {
                Name = name,
                City = city,
                Description = description,
                Year = year,
                TeamLogo = teamLogo,
                GroupId= groupId,
                TrenerId = trenerId
            };

            this.data.Teams.Add(teamData);
            this.data.SaveChanges();

            return teamData.Id;
        }

        public bool Edit(int id, string name, string city, string description, string teamLogo, int year, int groupId)
        {
            var teamData = this.data.Teams.Find(id);

            if (teamData == null)
            {
                return false;
            }

            teamData.Name = name;
            teamData.City = city;
            teamData.Description = description;
            teamData.TeamLogo = teamLogo;
            teamData.Year = year;
            teamData.GroupId = groupId;

            this.data.SaveChanges();

            return true;
        }

        public IEnumerable<TeamServiceModel> IsByUser(string userId)
            => GetTeams(this.data
                .Teams
                .Where(c => c.Trener.UserId == userId));

        public bool IsByTrener(int teamId, int trenerId)
            => this.data
                .Teams
                .Any(c => c.Id == teamId && c.TrenerId == trenerId);

        public IEnumerable<string> AllTeamNames()
            => this.data
                .Teams
                .Select(c => c.Name)
                .Distinct()
                .OrderBy(br => br)
                .ToList();

        public IEnumerable<TeamGroupServiceModel> AllGroups()
             => this.data
                 .Groups
                 .Select(c => new TeamGroupServiceModel
                 {
                     Id = c.Id,
                     Name = c.Name
                 })
                 .ToList();

        public bool GroupExists(int groupId)
            => this.data
                .Groups
                .Any(c => c.Id == groupId);


        private static IEnumerable<TeamServiceModel> GetTeams(IQueryable<Team> teamQuery)
           => teamQuery
               .Select(c => new TeamServiceModel
               {
                   Id = c.Id,
                   Name = c.Name,
                   City = c.City,
                   Year = c.Year,
                   TeamLogo = c.TeamLogo,
                   GroupName = c.Group.Name
               })
               .ToList();

    }
}
