namespace Turnir.Data.Models
{
    using System.Collections.Generic;

    public class Group
    {
        public int Id { get; init; }

        public string Name { get; set; }

        public IEnumerable<Team> Teams { get; set; } = new List<Team>();
    }
}
