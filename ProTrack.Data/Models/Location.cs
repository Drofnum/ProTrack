using System;
using System.Collections.Generic;
using System.Text;

namespace ProTrack.Data.Models
{
    public class Location
    {
        public int Id { get; set; }
        public string LocationName { get; set; }
        public string MyDotEmail { get; set; }
        public string C4AccountName { get; set; }

        public string ApplicationUser { get; set; }

        public IEnumerable<Device> Devices { get; set; }
    }
}
