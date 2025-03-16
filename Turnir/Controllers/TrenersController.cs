namespace Turnir.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Linq;
    using Turnir.Data;
    using Turnir.Data.Models;
    using Turnir.Infrastructure;
    using Turnir.Models.Treners;

    public class TrenersController : Controller
    {
        private readonly TurnirDbContext data;

        public TrenersController(TurnirDbContext data)
        {
            this.data = data;
        }

        [Authorize]
        public IActionResult Become() => View();

        [HttpPost]
        [Authorize]
        public IActionResult Become(BecomeTrenerFormModel trener)
        {
            var userId = this.User.GetId();

            var userIsAlreadyTrener = this.data
                .Treners
                .Any(d => d.UserId == userId);

            if (userIsAlreadyTrener)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(trener);
            }

            var trenerData = new Trener
            {
                Name = trener.Name,
                PhoneNumber = trener.PhoneNumber,
                UserId = userId
            };

            this.data.Treners.Add(trenerData);
            this.data.SaveChanges();

            return RedirectToAction("All", "Teams");
        }
    }
}
