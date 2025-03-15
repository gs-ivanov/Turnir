namespace Turnir.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Linq;
    using Turnir.Data;
    using Turnir.Data.Models;
    using Turnir.Models.Teams;

    public class TeamsController : Controller
    {
        private readonly TurnirDbContext data;

        public TeamsController(TurnirDbContext data)
        {
            this.data = data;
        }
        public IActionResult All([FromQuery] AllTeamsQueryModel query)
        {
            var teamsQuery = this.data.Teams.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.Name))
            {
                teamsQuery = teamsQuery.Where(t => t.Name == query.Name);
            }

            if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            {
                teamsQuery = teamsQuery.Where(t => (t.Name + " " + t.City).ToLower().Contains(query.SearchTerm.ToLower()) || t.Description.ToLower().Contains(query.SearchTerm.ToLower()));
            }

            teamsQuery = query.Sorting switch
            {
                TeamSorting.Year => teamsQuery.OrderByDescending(t => t.Year),
                TeamSorting.NameAndCity => teamsQuery.OrderBy(t => t.Name).ThenBy(t => t.City),
                TeamSorting.DateCreated or _ => teamsQuery.OrderByDescending(t => t.Id)
            };

            var totalTeams = teamsQuery.Count();

            var teams = teamsQuery
                .Skip((query.CurrentPage - 1) * AllTeamsQueryModel.TeamsPerPage)
                .Take(AllTeamsQueryModel.TeamsPerPage)
                .Select(t => new TeamListingViewModel
                {
                    Id = t.Id,
                    Name = t.Name,
                    City = t.City,
                    Year = t.Year,
                    TeamLogo = t.TeamLogo,
                    PointsWin=t.PointsWin,
                    PointsLost=t.PointsLost,
                    Group = t.Group.Name
                })
                 .ToList();

            var teamNames = this.data
                .Teams
                .Select(t => t.Name)
                .Distinct()
                .OrderBy(nm => nm)
                .ToList();

            query.TotalTeams = totalTeams;
            query.Names = teamNames;
            query.Teams = teams;

            return View(query);
        }

        public IActionResult Add() => View(new AddTeamFormModel
        {
            Groups = this.GetTeamGroups()
        });

        [HttpPost]
        public IActionResult Add(AddTeamFormModel team)
        {
            if (!this.data.Groups.Any(g => g.Id == team.GroupId))
            {
                this.ModelState.AddModelError(nameof(team.GroupId), "Group does not exist.");
            }

            if (!ModelState.IsValid)
            {
                team.Groups = this.GetTeamGroups();

                return View(team);
            }

            var teamData = new Team
            {
                Name = team.Name,
                City = team.City,
                Description = team.Description,
                TeamLogo = team.TeamLogo,
                Year = team.Year,
                PointsWin=team.PointsWin,
                PointsLost=team.PointsLost,
                GroupId = team.GroupId,
            };

            this.data.Teams.Add(teamData);
            this.data.SaveChanges();

            return RedirectToAction(nameof(All));

        }

        private IEnumerable<TeamGroupViewModel> GetTeamGroups()
             => this.data
            .Groups
            .Select(g => new TeamGroupViewModel
            {
                Id = g.Id,
                Name = g.Name
            })
            .ToList();
    }
}
