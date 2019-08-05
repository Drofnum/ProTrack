using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
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


        [HttpPost]
        public async Task<IActionResult> Create(EndUserEntryModel model, int locationId)
        {
            var userId = _userManager.GetUserId(User);

            if (locationId == 0 || model.Id == 0)
            {
                return NotFound();
            }

            var location = _context.Locations.Where(l => l.Id == locationId).FirstOrDefault();
            var save = BuildDeviceEntry(model, location);


            _deviceService.Create(save).Wait();

            return RedirectToAction("Entry", "EndUser", new { id = locationId });
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var device = await _context.Devices
                .Include(d => d.Location)
                .Include(d => d.Product)
                .ThenInclude(p => p.Manufacturer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (device == null)
            {
                return NotFound();
            }

            return View(device);
        }


        // GET: Devices/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var device = _deviceService.GetById(id);

            var mfgList = _context.Manufacturers
                .Select(mfg => new SelectListItem()
                {
                    Value = mfg.Id.ToString(),
                    Text = mfg.ManufacturerName,
                    //Selected = device.Product.Manufacturer.Id == mfg.Id ? true : false
                }).ToList();

            var productList = _context.Products
                .Select(p => new SelectListItem()
                {
                    Value = p.Id.ToString(),
                    Text = p.ProductName,
                    //Selected = device.Product.Id == p.Id ? true : false
                }).ToList();

            var model = new DeviceListingModel
            {
                Id = device.Id,
                ManufacturerName = device.Product.Manufacturer.ManufacturerName, //need to create IEnumerable for HTML select
                ManufacturerId = device.Product.Manufacturer.Id,
                Product = device.Product,
                Location = device.Location,
                MacAddress = device.MacAddress,
                Firmware = device.Firmware,
                Quantity = device.Quantity,
                ManufacturerList = mfgList,
                ProductList = productList
            };



            if (device == null)
            {
                return NotFound();
            }
            return View(model);
        }



        // POST: Devices/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EndUserEntryModel model, [Bind("Id,Firmware,MacAddress,Product,Location,Quantity")] Device device,int id)
        {
            var locationId = _context.Devices.Where(d => id == d.Id)
                .Select(d => d.Location.Id).FirstOrDefault();

            if (id != device.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    /*var location = _context.Locations.Where(l => l.Id == locationId).FirstOrDefault();
                    var save = BuildDeviceUpdate(model);
                    */
                    _context.Update(device);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DeviceExists(device.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Entry", "EndUser", new { id = locationId });
            }
            return View(device);
        }

        // GET: Devices/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var device = _deviceService.GetById(id);
            if (device == null)
            {
                return NotFound();
            }

            return View(device);
        }

        // POST: Devices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var locationId = _context.Devices.Where(d => id == d.Id)
                .Select(d => d.Location.Id).FirstOrDefault();
            var device = await _context.Devices.FindAsync(id);
            _context.Devices.Remove(device);
            await _context.SaveChangesAsync();
            return RedirectToAction("Entry", "EndUser", new { id = locationId });
        }



        [HttpPost]
        public IActionResult GetDevices(int Id)
        {
            return RedirectToAction("Entry", "EndUser", new { id = Id });
        }

        [HttpGet]
        public JsonResult GetProducts(int deviceType)
        {
            List<SelectListItem> productList = new List<SelectListItem>();

            productList = _context.Products.Where(dt => dt.DeviceType.Id == deviceType)
                .Select(p => new SelectListItem()
                {
                    Value = p.Id.ToString(),
                    Text = p.Manufacturer.ManufacturerName + " " + p.ProductName
                }).OrderBy(p => p.Text).ToList();           

            return Json(productList);
        }

        [HttpGet]
        public JsonResult GetProductsByMfg(int mfgId, int deviceId)
        {
            List<SelectListItem> selectedProduct = new List<SelectListItem>();
            List<SelectListItem> productList = new List<SelectListItem>();
            var productId = _context.Devices.Where(d => d.Id == deviceId)
                .Select(d => d.Product.Id)
                .FirstOrDefault();

            productList = _context.Products.Where(p => p.Manufacturer.Id == mfgId)
                .Select(p => new SelectListItem()
                {
                    Value = p.Id.ToString(),
                    Text = p.ProductName,
                    Selected = p.Id == productId - 1 ? true : false                    
                }).OrderBy(p => p.Selected).ToList();

            return Json(productList);
        }


        private bool DeviceExists(int id)
        {
            return _context.Devices.Any(e => e.Id == id);
        }

        private Device BuildDeviceEntry(EndUserEntryModel model, Location location)
        {
            var product = _context.Products.Where(p => p.Id == model.Id).FirstOrDefault();

            return new Device
            {
                Product = product,
                Quantity = 1,
                Location = location
            };
        }
    }
}