﻿namespace Turnir.Models.Teams
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Turnir.Services.Teams;

    public class AllTeamsQueryModel
    {
        public const int TeamsPerPage = 3;

        public string Name { get; init; }

        [Display(Name = "Search by text")]
        public string SearchTerm { get; init; }

        public TeamSorting Sorting { get; init; }

        public int CurrentPage { get; init; } = 1;

        public int TotalTeams { get; set; }

        public IEnumerable<string> Names { get; set; }

        public IEnumerable<TeamServiceModel> Teams { get; set; }
    }
}
