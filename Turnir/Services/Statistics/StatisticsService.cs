namespace Turnir.Services.Statistics
{
    using System;
    using System.Linq;
    using Turnir.Data;

    public class StatisticsService : IStatisticsService
    {
        private readonly TurnirDbContext data;

        public StatisticsService(TurnirDbContext data)
        {
            this.data = data;
        }
        public StatisticsServiceModel Total()
        {
            var totalTeams=this.data.Teams.Count();
            var totalUsers=this.data.Users.Count();

            return new StatisticsServiceModel
            {
                TotalTeams = totalTeams,
                TotalUsers = totalUsers,
                TotalRents = 0
            };
        }
    }
}
