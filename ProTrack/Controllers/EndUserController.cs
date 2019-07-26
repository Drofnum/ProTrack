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
using ProTrack.Models.Display;
using ProTrack.Models.EndUser;

namespace ProTrack.Controllers
{
    public class EndUserController : Controller
    {
        private readonly IEntry _entryService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IDevice _deviceService;

        public EndUserController(IEntry entryService, UserManager<ApplicationUser> userManager, ApplicationDbContext context, IDevice deviceService)
        {
            _entryService = entryService;
            _userManager = userManager;
            _context = context;
            _deviceService = deviceService;
        }


        public IActionResult Entry(int id)
        {
            var userId = _userManager.GetUserId(User);

            var locationList = _context.Locations.Where(l => l.ApplicationUser == userId)
                .Select(l => new SelectListItem()
                {
                    Value = l.Id.ToString(),
                    Text = l.LocationName,
                    Selected = l.Id == id ? true : false
                }).ToList();

            var deviceTypeList = _context.DeviceTypes
                .Select(dt => new SelectListItem()
                {
                    Value = dt.Id.ToString(),
                    Text = dt.Type
                }).ToList();

            /*
            var mfgList = _context.Manufacturers
                .Select(mfg => new SelectListItem()
                {
                    Value = mfg.Id.ToString(),
                    Text = mfg.ManufacturerName
                }).ToList();

            */
            var deviceList = _context.Devices.Where(l => l.Location.ApplicationUser == userId && l.Location.Id == id) //_deviceService.GetAll()
                .Select(device => new DeviceListingModel
                {
                    Id = device.Id,
                    ManufacturerName = device.Product.Manufacturer.ManufacturerName,
                    ProductName = device.Product.ProductName,
                    MacAddress = device.MacAddress,
                    Firmware = device.Firmware,
                    Quantity = device.Quantity
                });

            var model = new EndUserEntryModel
            {
                LocationList = locationList,
                DeviceTypeList = deviceTypeList,
                //ManufacturerList = mfgList,
                DeviceList = deviceList

            };

            return View(model);
        }

        /*
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
        */

        [HttpGet]
        public JsonResult GetManufacturer(int deviceType)
        {
            List<SelectListItem> mfgList = new List<SelectListItem>();

            mfgList = _context.Products.Where(dt => dt.DeviceType.Id == deviceType)
                .Select(mfg => new SelectListItem()
                {
                    Value = mfg.Id.ToString(),
                    Text = mfg.Manufacturer.ManufacturerName + " " + mfg.ProductName
                }).ToList();

            return Json(mfgList);
        }

        [HttpPost]
        public IActionResult GetDevices(int Id)
        {
            return RedirectToAction("Entry", "EndUser", new { id = Id });
        }

        [HttpPost]
        public async Task<IActionResult> Create(EndUserEntryModel model, int locationId)
        {
            var userId = _userManager.GetUserId(User);
            var save = BuildDeviceEntry(model);
            var location = model.LocationId;

            _deviceService.Create(save).Wait();

            return RedirectToAction("Entry", "EndUser", new { id = location });
        }

        private Device BuildDeviceEntry(EndUserEntryModel model)
        {
            var product = _context.Products.Where(p => p.Id == model.Id).FirstOrDefault();
            
            return new Device
            {
                Product = product,
                Quantity = 1,
                Location = model.Location
            };
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