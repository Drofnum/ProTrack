using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProTrack.Models.Display
{
    public class ProductListingModel
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string ManufacturerName { get; set; }
        public string DeviceType { get; set; }
    }
}
