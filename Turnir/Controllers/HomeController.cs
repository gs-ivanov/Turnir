namespace Turnir.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.Linq;
    using Turnir.Data;
    using Turnir.Models.Home;
    using Turnir.Services.Statistics;

    public class HomeController : Controller
    {
        private readonly IStatisticsService statistics;
        private readonly TurnirDbContext data;

        public HomeController(IStatisticsService statistics, TurnirDbContext data)
        {
            this.statistics = statistics;
            this.data = data;
        }


        public IActionResult Index()
        {

            var teams = this.data
                .Teams
                .OrderByDescending(t => t.Id)
                .Select(t => new TeamIndexViewModel
                {
                    Id = t.Id,
                    Name = t.Name,
                    City = t.City,
                    Year = t.Year,
                    TeamLogo = t.TeamLogo,
                    PointsWin = t.PointsWin,
                    PointsLost = t.PointsLost
                })
                .Take(3)
                .ToList();

            var totalStatistics = this.statistics.Total();

            return View(new IndexViewModel
            {
                TotalTeams = totalStatistics.TotalTeams,
                TotalUsers = totalStatistics.TotalUsers,
                Teams = teams
            });
        }
        public IActionResult Error() => View();
    }
}

