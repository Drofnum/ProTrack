using Microsoft.AspNetCore.Mvc.Rendering;
using ProTrack.Data.Models;
using ProTrack.Models.Display;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProTrack.Models.Products
{
    public class ProductsEntryModel
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public int DTId { get; set; }
        public int MfgId { get; set; }

        public DeviceType DeviceType { get; set; }
        public Manufacturer Manufacturer { get; set; }

        public IEnumerable<ProductListingModel> ProductList { get; set; }

        public List<SelectListItem> ManufacturerList { get; set; }
        public List<SelectListItem> DeviceTypeList { get; set; }
    }
}
