namespace Turnir.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Linq;
    using Turnir.Data;
    using Turnir.Data.Models;
    using Turnir.Infrastructure;
    using Turnir.Models.Teams;
    using Turnir.Models;
    using Turnir.Services.Teams;

    public class TeamsController : Controller
    {
        private readonly ITeamService teams;
        private readonly TurnirDbContext data;

        public TeamsController(ITeamService teams, TurnirDbContext data)
        {
            this.teams = teams;
            this.data = data;
        }

        public IActionResult All([FromQuery] AllTeamsQueryModel query)
        {
            var queryResult = this.teams.All(
                query.Name,
                query.SearchTerm,
                query.Sorting,
                query.CurrentPage,
                AllTeamsQueryModel.TeamsPerPage);

            var teamNames = this.teams.AllTeamNames();

            query.Names = teamNames;
            query.TotalTeams = queryResult.TotalTeams;
            query.Teams = queryResult.Teams;

            return View(query);
        }


        //    if (!string.IsNullOrWhiteSpace(query.Name))
        //    {
        //        queryResult = queryResult.Where(t => t.Name == query.Name);
        //    }

        //    if (!string.IsNullOrWhiteSpace(query.SearchTerm))
        //    {
        //        queryResult = queryResult.Where(t => (t.Name + " " + t.City).ToLower().Contains(query.SearchTerm.ToLower()) || t.Description.ToLower().Contains(query.SearchTerm.ToLower()));
        //    }

        //    queryResult = query.Sorting switch
        //    {
        //        TeamSorting.Year => queryResult.OrderByDescending(t => t.Year),
        //        TeamSorting.NameAndCity => queryResult.OrderBy(t => t.Name).ThenBy(t => t.City),
        //        TeamSorting.DateCreated or _ => queryResult.OrderByDescending(t => t.Id)
        //    };

        //    var totalTeams = queryResult.Count();

        //    var teams = queryResult
        //        .Skip((query.CurrentPage - 1) * AllTeamsQueryModel.TeamsPerPage)
        //        .Take(AllTeamsQueryModel.TeamsPerPage)
        //        .Select(t => new TeamListingViewModel
        //        {
        //            Id = t.Id,
        //            Name = t.Name,
        //            City = t.City,
        //            Year = t.Year,
        //            TeamLogo = t.TeamLogo,
        //            PointsWin = t.PointsWin,
        //            PointsLost = t.PointsLost,
        //            Group = t.Group.Name
        //        })
        //         .ToList();

        //    var teamNames = this.data
        //        .Teams
        //        .Select(t => t.Name)
        //        .Distinct()
        //        .OrderBy(nm => nm)
        //        .ToList();

        //    query.TotalTeams = totalTeams;
        //    query.Names = teamNames;
        //    query.Teams = teams;

        //    return View(query);
        //}

        [Authorize]
        public IActionResult Add()
        {
            if (this.UserIsTrener()==0)
            {
                return RedirectToAction(nameof(TrenersController.Become), "Treners");
            }
            if (TrenerAllreadyHasTeam())
            {
                return RedirectToAction(nameof(All), "Teams");
            }
            return View(new AddTeamFormModel
            {
                Groups = this.GetTeamGroups(),
                TrenerId = this.UserIsTrener()
            }); ;
        }

        [HttpPost]
        [Authorize]
        public IActionResult Add(AddTeamFormModel team)
        {
            if (TrenerAllreadyHasTeam())
            {

            }

            if (this.UserIsTrener()==0)
            {
                return RedirectToAction(nameof(TrenersController.Become), "Treners");
            }
            else
            {
                var currId = this.UserIsTrener();
            }

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
                PointsWin = team.PointsWin,
                PointsLost = team.PointsLost,
                GroupId = team.GroupId,
                TrenerId=team.TrenerId
            };

            this.data.Teams.Add(teamData);
            this.data.SaveChanges();

            return RedirectToAction(nameof(All));

        }

        private int UserIsTrener()
            => (this.data
                .Treners
                .Where(t => t.UserId == this.User.GetId())
                .Select(t=>t.Id)
                .FirstOrDefault());

        private bool TrenerAllreadyHasTeam()
        {
            var x = this.data
                .Teams
                .Any(t => t.TrenerId == UserIsTrener());
            return x;
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
