namespace Turnir.Models.Teams
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Turnir.Models.Treners;
    using static Data.DataConstants.Team;

    public class AddTeamFormModel
    {
        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string Name { get; init; }

        [Required]
        [StringLength(CityMaxLength, MinimumLength = CityMinLength)]
        public string City { get; init; }

        [Required]
        [StringLength(
            int.MaxValue,
            MinimumLength = DescriptionMinLength,
            ErrorMessage = "The field Description must be a string with a minimum length of {2}.")]
        public string Description { get; init; }

        [Display(Name = "Team Logo")]
        [Required]
        public string TeamLogo { get; init; }

        [Range(YearMinValue, YearMaxValue)]
        [Display(Name = "Team established at Year:")]
        public int Year { get; init; }

        [Display(Name = "Winning points:")]
        public int PointsWin { get; init; }

        [Display(Name = "Lost points:")]
        public int PointsLost { get; init; }

        [Display(Name = "Group")]
        public int GroupId { get; init; }

        public IEnumerable<TeamGroupViewModel> Groups { get; set; }

        [Display(Name = "Trener")]
        public int TrenerId { get; init; }

        public IEnumerable<BecomeTrenerFormModel> Trener { get; set; }

    }
}
