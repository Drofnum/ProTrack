using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProTrack.Data;
using ProTrack.Data.Models;
using ProTrack.Models.Display;

namespace ProTrack.Controllers
{
    public class LocationController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public LocationController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
                _userManager = userManager;
                _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ManageLocation()
        {
            var userId = _userManager.GetUserId(User);
            var locationList = _context.Locations.Where(l => l.ApplicationUser == userId)
                .Select(location => new LocationListingModel
                {
                    Id = location.Id,
                    LocationName = location.LocationName,
                    MyDotEmail = location.MyDotEmail,
                    C4AccountName = location.C4AccountName
                });

            var model = new LocationIndexModel
            {
                LocationList = locationList
            };

            return View(model);
        }


        public IActionResult CreateLocation()
        {
            return View();
        }

        // POST: Devices/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateLocation([Bind("Id,LocationName,MyDotEmail,C4AccountName,ApplicationUser")] Location location)
        {
            var userId = _userManager.GetUserId(User);
            var saveLocation = new Location
            {
                LocationName = location.LocationName,
                MyDotEmail = location.MyDotEmail,
                C4AccountName = location.C4AccountName,
                ApplicationUser = userId
            };

            if (ModelState.IsValid)
            {
                _context.Add(saveLocation);
                await _context.SaveChangesAsync();
                return RedirectToAction("Entry", "EndUser", new { id = saveLocation.Id });
            }
            return View(location);
        }
    }
}