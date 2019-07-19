using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProTrack.Models.Display
{
    public class ProductIndexModel
    {
        public IEnumerable<ProductListingModel> ProductList { get; set; }
    }
}
