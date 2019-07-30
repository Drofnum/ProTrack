using Microsoft.AspNetCore.Mvc.Rendering;
using ProTrack.Data.Models;
using ProTrack.Models.Display;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProTrack.Models.EndUser
{
    public class EndUserEntryModel
    {
        public int Id { get; set; }
        public string ManufacturerName { get; set; }
        public string ProductName { get; set; }
        public string MacAddress { get;  set; }
        public string Firmware { get; set; }
        public int Quantity { get; set; }
        public string LocationName { get; set; }
        public int LocationId { get; set; }
        public Device Device { get; set; }
        public Manufacturer Manufacturer { get; set; }
        public Product Product { get; set; }
        public Location Location { get; set; }
        public DeviceType DeviceType { get; set; }

        public List<SelectListItem> LocationList { get; set; }
        public List<SelectListItem> ManufacturerList { get; set; }
        public List<SelectListItem> DeviceTypeList { get; set; }
        public List<SelectListItem> ProductList { get; set; }
        public IEnumerable<DeviceListingModel> DeviceList { get; set; }
        
    }
}
