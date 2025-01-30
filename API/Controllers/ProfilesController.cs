using Application.Profiles;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ProfilesController : BaseApiController
    {
        [HttpGet("{username}")]
        public async Task<IActionResult> GetProfile(string username)
        {
            return HandleResult(await Mediator.Send(new Details.Query { UserName = username }));
        }

        [HttpGet("{username}/activities")]
        public async Task<IActionResult> GetActivities(string username, [FromQuery] string predicate)
        {
            return HandleResult(await Mediator.Send(new ListActivities.Query { Username = username, Predicate = predicate }));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProfile(Profile profile)
        {
            return HandleResult(await Mediator.Send(new Edit.Command { Profile = profile }));
        }
    }
}