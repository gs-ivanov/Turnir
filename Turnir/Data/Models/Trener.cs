namespace Turnir.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static DataConstants.Trener;

    public class Trener
    {
        public int Id { get; init; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; }

        public int TeamId { get; init; }

        [Required]
        [MaxLength(PhoneNumberMaxLength)]
        public string PhoneNumber { get; set; }


        [Required]
        public string UserId { get; set; }

        public IEnumerable<Team> Teams { get; init; } = new List<Team>();

    }
}
