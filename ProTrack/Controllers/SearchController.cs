using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProTrack.Data;
using ProTrack.Data.Models;
using ProTrack.Models.Search;

namespace ProTrack.Controllers
{
    public class SearchController : Controller
    {
        private readonly IDevice _deviceService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public SearchController(IDevice deviceService, UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _deviceService = deviceService;
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> Index(string emailSearchString, string productSearchString)
        {
            IQueryable<string> productQuery = from p in _context.Products
                                              orderby p.ProductName
                                              select p.ProductName;

            IQueryable<Device> devices = _context.Devices;

            if (!String.IsNullOrEmpty(emailSearchString))
            {
                var userEmail = _userManager.Users.Where(u => u.Email.Contains(emailSearchString)).Select(u => u.Id).FirstOrDefault();
                devices = devices.Where(s => s.Location.ApplicationUser == userEmail);
            }
            if (!String.IsNullOrEmpty(productSearchString))
            {
                devices = devices.Where(p => p.Product.ProductName.Contains(productSearchString));
            }
            var areNoResults = (!string.IsNullOrEmpty(productSearchString) && !devices.Any());

            var deviceListing = devices.Select(device => new SearchListingModel
            {
                Id = device.Id,
                ManufacturerName = device.Product.Manufacturer.ManufacturerName,
                ProductName = device.Product.ProductName,
                MacAddress = device.MacAddress,
                Firmware = device.Firmware,
                Quantity = device.Quantity,
                LocationName = device.Location.LocationName,
                Email = _userManager.Users.Where(u => u.Id == device.Location.ApplicationUser).Select(u => u.Email).FirstOrDefault()

        });

            var model = new SearchResultModel
            {
                Devices = deviceListing,
                Products = new SelectList(await productQuery.Distinct().ToListAsync()),
                EmptySearchResults = areNoResults                
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Search(string searchQuery)
        {
            return RedirectToAction("Results", new { searchQuery });
        }
    }
}