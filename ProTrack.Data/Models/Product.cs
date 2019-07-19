using System;
using System.Collections.Generic;
using System.Text;

namespace ProTrack.Data.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string ProductName { get; set; }

        public DeviceType DeviceType { get; set; }
        public Manufacturer Manufacturer { get; set; }
    }
}
