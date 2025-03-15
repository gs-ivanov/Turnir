namespace Turnir.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using static DataConstants.Team;

    public class Team
    {
        public int Id { get; init; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; }

        [Required]
        [MaxLength(CityMaxLength)]
        public string City { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string TeamLogo { get; set; }

        public int Year { get; set; }

        public bool IsPublic { get; set; }

        public int PointsWin { get; set; }

        public int PointsLost { get; set; }

        public int GroupId { get; set; }

        public Group Group { get; init; }

    }
}
