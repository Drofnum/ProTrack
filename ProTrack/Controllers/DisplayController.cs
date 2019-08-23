using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProTrack.Data;
using ProTrack.Data.Models;
using ProTrack.Models.Display;

namespace ProTrack.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DisplayController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IDevice _deviceService;
        private readonly UserManager<ApplicationUser> _userManager;

        public DisplayController(ApplicationDbContext context, IDevice deviceService, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _deviceService = deviceService;
            _userManager = userManager;
        }

        public IActionResult Device()
        {
            var deviceList = _context.Devices //_deviceService.GetAll()
                .Select(device => new DeviceListingModel
                {
                    Id = device.Id,
                    ManufacturerName = device.Product.Manufacturer.ManufacturerName,
                    ProductName = device.Product.ProductName,
                    MacAddress = device.MacAddress,
                    Firmware = device.Firmware,
                    Quantity = device.Quantity
                });

            var model = new DeviceIndexModel
            {
                DeviceList = deviceList
            };

            return View(model);
        }

        public IActionResult DeviceType()
        {
            var dtList = _context.DeviceTypes
                .Select(dt => new DeviceTypeListingModel
                {
                    Id = dt.Id,
                    DeviceType = dt.Type
                });

            var model = new DeviceTypeIndexModel
            {
                DeviceTypeList = dtList
            };

            return View(model);
        }

        public IActionResult Location()
        {
            var locationList = _context.Locations
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

        public IActionResult Manufacturer()
        {
            var mfgList = _context.Manufacturers
                .Select(mfg => new ManufacturerListingModel
                {
                    Id = mfg.Id,
                    ManufacturerName = mfg.ManufacturerName
                });

            var model = new ManufacturerIndexModel
            {
                ManufacturerList = mfgList
            };

            return View(model);
        }

        public IActionResult Product()
        {
            var productList = _context.Products
                .Select(product => new ProductListingModel
                {
                    Id = product.Id,
                    ProductName = product.ProductName
                });

            var model = new ProductIndexModel
            {
                ProductList = productList
            };

            return View(model);
        }
    }
}