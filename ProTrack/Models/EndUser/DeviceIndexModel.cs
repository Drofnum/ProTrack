using ProTrack.Models.Display;
using System.Collections.Generic;

namespace ProTrack.Models.EndUser
{
    public class EntryIndexModel
    {
        public IEnumerable<LocationListingModel> LocationList { get; set; }
        public IEnumerable<ManufacturerListingModel> ManufacturerList { get; set; }
    }
}
