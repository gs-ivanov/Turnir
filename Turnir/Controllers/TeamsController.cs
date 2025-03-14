namespace Turnir.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System;
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
        public IActionResult Add()=>  View(new AddTeamFormModel
        {
            Groups=this.GetTeamGroups()
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
                GroupId = team.GroupId,
            };

            this.data.Teams.Add(teamData);
            this.data.SaveChanges();

            return RedirectToAction("Index", "Home");

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
