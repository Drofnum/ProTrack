using ProTrack.Data.Models;
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
        public string LocationName { get; set; }
        public Device Device { get; set; }
        public Manufacturer Manufacturer { get; set; }
        public Product Product { get; set; }
        public Location Location { get; set; }
    }
}
