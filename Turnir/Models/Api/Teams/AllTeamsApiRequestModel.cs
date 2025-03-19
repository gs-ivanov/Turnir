namespace Turnir.Models.Api.Teams
{
    public class AllTeamsApiRequestModel
    {
        public string Name { get; init; }

        public string SearchTerm { get; init; }

        public TeamSorting Sorting { get; init; }

        public int CurrentPage { get; init; } = 1;

        public int TeamsPerPage { get; init; } = 10;
    }
}
