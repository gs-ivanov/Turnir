namespace Turnir.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.Diagnostics;
    using System.Linq;
    using Turnir.Data;
    using Turnir.Models;
    using Turnir.Models.Home;

    public class HomeController : Controller
    {
        private readonly TurnirDbContext data;

        public HomeController(TurnirDbContext data)
        {
            this.data = data;
        }


        public IActionResult Index() 
        {
            var totalTeams = this.data.Teams.Count();

            var teams = this.data
                .Teams
                .OrderByDescending(t => t.Id)
                .Select(t => new TeamIndexViewModel
                {
                    Id=t.Id,
                    Name = t.Name,
                    City = t.City,
                    Year = t.Year,
                    TeamLogo = t.TeamLogo,
                    PointsWin = t.PointsWin,
                    PointsLost = t.PointsLost
                })
                .Take(3)
                .ToList();

            return View(new IndexViewModel
            {
                TotalTeams=totalTeams,
                Teams=teams
            });
        }
    

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()=> View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

