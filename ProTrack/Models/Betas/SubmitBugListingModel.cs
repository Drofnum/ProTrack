using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProTrack.Models.Betas
{
    public class SubmitBugListingModel
    {
        public int Id { get; set; }
        [Display(Name = "Project Name")]
        public string ProjectName { get; set; }
        [Display(Name = "Bug Summary")]
        public string BugSummary { get; set; }
        [Display(Name = "Bug Description")]
        public string BugDescription { get; set; }
        [Display(Name = "Build or Firmware number")]
        public string BuildNumber { get; set; }
    }
}
