namespace Turnir.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Linq;
    using Turnir.Infrastructure;
    using Turnir.Models.Teams;
    using Turnir.Services.Teams;
    using Turnir.Services.Treners;

    public class TeamsController : Controller
    {
        private readonly ITeamService teams;
        private readonly ITrenerService treners;

        public TeamsController(
            ITeamService teams,
            ITrenerService treners)
        {
            this.teams = teams;
            this.treners = treners;
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

        [Authorize]
        public IActionResult Mine()
        {
            var myTeams = this.teams.IsByUser(this.User.Id());

            return View(myTeams);
        }

        [Authorize]
        public IActionResult Add()
        {
            if (this.treners.IsTrener(this.User.Id()))
            {
                return RedirectToAction(nameof(Mine), "Teams");
            }

            if (this.treners.TrenerAllreadyHasTeam(this.User.Id()))
            {
                return RedirectToAction(nameof(All), "Teams");
            }

            return View(new TeamFormModel
            {
                Groups = this.teams.AllGroups(),
                TrenerId = this.treners.IsByUser(this.User.Id())
            });
        }

        [HttpPost]
        [Authorize]
        public IActionResult Add(TeamFormModel team)
        {
            var trenerId = this.treners.IsByUser(this.User.Id());

            if (trenerId == 0)
            {
                return RedirectToAction(nameof(TrenersController.Become), "Treners");
            }

            if (!this.teams.GroupExists(team.GroupId))
            {
                this.ModelState.AddModelError(nameof(team.GroupId), "Group does not exist.");
            }

            if (!ModelState.IsValid)
            {
                team.Groups = this.teams.AllGroups();

                return View(team);
            }

            this.teams.Create(
                team.Name,
                team.City,
                team.Description,
                team.TeamLogo,
                team.Year,
                team.GroupId,
                trenerId);

            return RedirectToAction(nameof(All));
        }

        [Authorize]
        public IActionResult Edit(int id)
        {
            var userId = this.User.Id();

            if (!this.treners.IsTrener(userId))
            {
                return RedirectToAction(nameof(TrenersController.Become), "Treners");
            }

            var team = this.teams.Details(id);

            if (team.UserId != userId)
            {
                return Unauthorized();
            }

            return View(new TeamFormModel
            {
                Name = team.Name,
                City = team.Name,
                Description = team.Description,
                TeamLogo = team.TeamLogo,
                Year = team.Year,
                GroupId = team.GroupId,
                Groups = this.teams.AllGroups()
            });
        }

        [HttpPost]
        [Authorize]
        public IActionResult Edit(int id, TeamFormModel team)
        {
            var trenerId = this.treners.IsByUser(this.User.Id());

            if (trenerId == 0)
            {
                return RedirectToAction(nameof(TrenersController.Become), "Treners");
            }

            if (!this.teams.GroupExists(team.GroupId))
            {
                this.ModelState.AddModelError(nameof(team.GroupId), "Group does not exist.");
            }

            if (!ModelState.IsValid)
            {
                team.Groups = this.teams.AllGroups();

                return View(team);
            }

            if (!this.teams.IsByTrener(id, trenerId))
            {
                return BadRequest();
            }

            this.teams.Edit(
                id,
                team.Name,
                team.City,
                team.Description,
                team.TeamLogo,
                team.Year,
                team.GroupId);

            return RedirectToAction(nameof(All));
        }


        //private int UserIsTrener()
        //    => (this.data
        //        .Treners
        //        .Where(t => t.UserId == this.User.GetId())
        //        .Select(t => t.Id)
        //        .FirstOrDefault());

        //private bool TrenerAllreadyHasTeam()
        //{
        //    var x = this.data
        //        .Teams
        //        .Any(t => t.TrenerId == UserIsTrener());
        //    return x;
        //}

        //private IEnumerable<TeamGroupServiceModel> GetTeamGroups()
        //     => this.data
        //    .Groups
        //    .Select(g => new TeamGroupServiceModel
        //    {
        //        Id = g.Id,
        //        Name = g.Name
        //    })
        //    .ToList();

    }
}
