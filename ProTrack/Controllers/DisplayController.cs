using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ProTrack.Data;
using ProTrack.Models.Display;

namespace ProTrack.Controllers
{
    public class DisplayController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IDevice _deviceService;

        public DisplayController(ApplicationDbContext context, IDevice deviceService)
        {
            _context = context;
            _deviceService = deviceService;
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
            return View();
        }

        public IActionResult Product()
        {
            return View();
        }
    }
}