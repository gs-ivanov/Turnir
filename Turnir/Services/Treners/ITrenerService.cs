namespace Turnir.Services.Treners
{
    public interface ITrenerService
    {
        public bool IsTrener(string userId);

        public bool TrenerAllreadyHasTeam(string userId);

        public int IsByUser(string userId);
    }
}
