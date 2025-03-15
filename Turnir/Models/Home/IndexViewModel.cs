namespace Turnir.Models.Home
{
    using System.Collections.Generic;

    public class IndexViewModel
    {
        public int TotalTeams { get; init; }

        public int TotalUsers { get; init; }

        public int TotalRents { get; init; }

        public List<TeamIndexViewModel> Teams { get; init; }
    }
}
