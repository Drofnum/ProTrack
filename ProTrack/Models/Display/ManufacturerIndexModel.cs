using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProTrack.Models.Display
{
    public class ManufacturerIndexModel
    {
        public IEnumerable<ManufacturerListingModel> ManufacturerList { get; set; }
    }
}
