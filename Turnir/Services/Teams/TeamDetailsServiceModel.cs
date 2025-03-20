namespace Turnir.Services.Teams
{
    public class TeamDetailsServiceModel : TeamServiceModel
    {
        public string Description { get; init; }

        public int GroupId { get; init; }

        public int TrenerId { get; init; }

        public string TrenerName { get; init; }

        public string UserId { get; init; }
    }
}
