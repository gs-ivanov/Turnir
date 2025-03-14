namespace Turnir.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static DataConstants.Group;

    public class Trener
    {
        public int Id { get; init; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; }

        public int TeamId { get; init; }

        [Required]
        public string UserId { get; set; }

        public IEnumerable<Team> Teams { get; init; } = new List<Team>();

    }
}
