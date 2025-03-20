namespace Turnir.Services.Treners
{
    using System.Linq;
    using Turnir.Data;

    public class TrenerService : ITrenerService
    {
        private readonly TurnirDbContext data;

        public TrenerService(TurnirDbContext data)
            => this.data = data;

        public bool IsTrener(string userId)
            => this.data
                .Treners
                .Any(d => d.UserId == userId);

        public int IsByUser(string userId)
            => this.data
                .Treners
                .Where(d => d.UserId == userId)
                .Select(d => d.Id)
                .FirstOrDefault();

        public bool TrenerAllreadyHasTeam(string userId)
        {
            var Id= this.data
                .Treners
                .Where(d => d.UserId == userId)
                .Select(d => d.Id)
                .FirstOrDefault();
            if (Id==0)
            {
                return false;
            }
            var x = this.data
                .Teams
                .Any(t => t.TrenerId == Id);

            return x;
        }



    }
}
