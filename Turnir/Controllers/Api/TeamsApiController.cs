namespace Turnir.Controllers.Api
{
    using Microsoft.AspNetCore.Mvc;
    using Turnir.Models.Api.Teams;
    using Turnir.Services.Teams;

    [ApiController]
    [Route("api/teams")]
    public class TeamsApiController : ControllerBase
    {
        private readonly ITeamService teams;

        public TeamsApiController(ITeamService teams)
        {
            this.teams = teams;
        }

        [HttpGet]
        public TeamQueryServiceModel All([FromQuery]
                    AllTeamsApiRequestModel query)
            => this.teams.All(
                query.Name,
                query.SearchTerm,
                query.Sorting,
                query.CurrentPage,
                query.TeamsPerPage);
    }
}
