using System;
using System.Collections.Generic;
using System.Text;

namespace ProTrack.Data.Models
{
    public class BetaOpportunity
    {
        public int Id { get; set; }
        public string ProjectName { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public string DriverUrl { get; set; }
        public string QuickStartGuideUrl { get; set; }
        public string UserGuideUrl { get; set; }
        public string FirmwareUrl { get; set; }
    }
}
