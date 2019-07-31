using Microsoft.AspNetCore.Mvc.Rendering;
using ProTrack.Data.Models;
using System.Collections.Generic;

namespace ProTrack.Models.Display
{
    public class DeviceListingModel
    {
        public int Id { get; set; }
        public string Firmware { get; set; }
        public string MacAddress { get; set; }
        public int Quantity { get; set; }
        public string ManufacturerName { get; set; }
        public int ManufacturerId { get; set; }
        public string ProductName { get; set; }
        public Product Product { get; set; }
        public Location Location { get; set; }


        public List<SelectListItem> LocationList { get; set; }
        public List<SelectListItem> ManufacturerList { get; set; }
        public List<SelectListItem> DeviceTypeList { get; set; }
        public List<SelectListItem> ProductList { get; set; }
    }
}
