using System;
using System.Collections.Generic;
using System.Text;

namespace ProTrack.Data.Models
{
    public class Device
    {
        public int Id { get; set; }
        public string Firmware { get; set; }
        public string MacAddress { get; set; }
        public int Quantity { get; set; }
        public Manufacturer Manufacturer { get; set; }
        public Product Product { get; set; }
    }
}
