using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProTrack.Models.BetaOpportunities
{
    public class BetaListingModel
    {
        public int Id { get; set; }
        [Display(Name = "Project Name")]
        public string ProjectName { get; set; }
        [Display(Name = "Description")]
        public string ShortDescription { get; set; }
        [Display(Name = "Long Description")]
        public string LongDescription { get; set; }
        public string DriverUrl { get; set; }
        public string QuickStartGuideUrl { get; set; }
        public string UserGuideUrl { get; set; }
        public string FirmwareUrl { get; set; }
    }
}
