using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ProTrack.Models.Search
{
    public class SearchResultModel
    {
        public IEnumerable<SearchListingModel> Devices { get; set; }
        public string emailSearchString { get; set; }
        public string productSearchString { get; set; }
        public bool EmptySearchResults { get; set; }
        public SelectList Products { get; set; }
    }
}
