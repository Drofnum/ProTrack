using ProTrack.Models.Display;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProTrack.Models.Search
{
    public class SearchResultModel
    {
        public IEnumerable<DeviceListingModel> Devices { get; set; }
        public string SearchQuery { get; set; }
        public bool EmptySearchResults { get; set; }
    }
}
