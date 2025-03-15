namespace Turnir.Models.Teams
{
    public class TeamListingViewModel
    {
        public int Id { get; init; }

        public string Name { get; init; }

        public string City { get; init; }

        public string TeamLogo { get; init; }

        public int Year { get; init; }

        public int PointsWin { get; init; }

        public int PointsLost { get; init; }

        public string Group { get; init; }
    }
}