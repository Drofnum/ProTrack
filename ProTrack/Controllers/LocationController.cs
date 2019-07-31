using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProTrack.Data;
using ProTrack.Data.Models;
using ProTrack.Models.Display;

namespace ProTrack.Controllers
{
    public class LocationController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILocation _locationService;

        public LocationController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, ILocation locationService)
        {
            _userManager = userManager;
            _locationService = locationService;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Manage()
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


        public IActionResult Create()
        {
            return View();
        }

        // POST: Location/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,LocationName,MyDotEmail,C4AccountName,ApplicationUser")] Location location)
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

        // GET: Location/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);
            var location = _locationService.GetById(id);

            var model = new LocationListingModel
            {
                Id = location.Id,
                LocationName = location.LocationName,
                MyDotEmail = location.MyDotEmail,
                C4AccountName = location.C4AccountName,
                ApplicationUser = userId
            };

            if (location == null)
            {
                return NotFound();
            }
            return View(model);
        }

        // POST: Location/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,LocationName,MyDotEmail,C4AccountName,ApplicationUser")] Location location)
        {
            if (id != location.Id)
            {
                return NotFound();
            }

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
                try
                {
                    _context.Update(saveLocation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LocationExists(location.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Manage));
            }
            return View(location);
        }

        // GET: Location/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var location = await _context.Locations
                .FirstOrDefaultAsync(m => m.Id == id);
            if (location == null)
            {
                return NotFound();
            }

            return View(location);
        }

        // POST: Location/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var location = await _context.Locations.FindAsync(id);
            _context.Locations.Remove(location);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Manage));
        }

        private bool LocationExists(int id)
        {
            return _context.Locations.Any(e => e.Id == id);
        }
    }
}