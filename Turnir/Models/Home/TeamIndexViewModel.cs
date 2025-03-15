namespace Turnir.Models.Home
{
    public class TeamIndexViewModel
    {
        public int Id { get; init; }

        public string Name { get; init; }

        public string City { get; init; }

        public string TeamLogo { get; init; }

        public int Year { get; init; }

        public int PointsWin { get; set; }

        public int PointsLost { get; set; }

    }
}