using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProTrack.Data;
using ProTrack.Data.Models;
using ProTrack.Models.EndUser;

namespace ProTrack.Controllers
{
    public class EndUserController : Controller
    {
        private readonly IEntry _entryService;
        private readonly UserManager<ApplicationUser> _userManager;

        public EndUserController(IEntry entryService, UserManager<ApplicationUser> userManager)
        {
            _entryService = entryService;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Entry(EndUserEntryModel model)
        {
            var userId = _userManager.GetUserId(User);
            var user = await _userManager.FindByIdAsync(userId);
            var entry = BuildEntry(model, user);
            var location = BuildLocation(model, user);

            _entryService.Add(entry).Wait();
            _entryService.Add(location).Wait();

            return RedirectToAction("Index", "EndUser", new { id = entry.Id });
        }

        private EndUserEntry BuildLocation(EndUserEntryModel model, ApplicationUser user)
        {
            return new EndUserEntry
            {
                Id = model.Location.Id
            };
        }

        private EndUserEntry BuildEntry(EndUserEntryModel model, ApplicationUser user)
        {
            return new EndUserEntry
            {
                Manufacturer = model.Manufacturer,
                Product = model.Product,
            };
        }
    }
}