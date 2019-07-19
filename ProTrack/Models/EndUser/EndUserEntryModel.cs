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
        public Device Device { get; set; }
        public Manufacturer ManuFacturer { get; set; }
        public Product Product { get; set; }

        public IEnumerable<Location> Location { get; set; }
    }
}
