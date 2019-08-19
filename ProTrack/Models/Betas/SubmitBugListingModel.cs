using System.ComponentModel.DataAnnotations;

namespace ProTrack.Models.Betas
{
    public class SubmitBugListingModel
    {
        public int Id { get; set; }
        [Display(Name = "Project Name")]
        [Required]
        public string ProjectName { get; set; }
        [Display(Name = "Bug Summary")]
        [Required]
        [StringLength(255, ErrorMessage = "{0} must be at least {2} characters long.", MinimumLength = 15)]
        [RegularExpression("^[-_, A-Za-z0-9]*$", ErrorMessage = "Bug Summary can not contain special characters")]
        public string BugSummary { get; set; }
        [Display(Name = "Bug Description")]
        [Required]
        [StringLength(32000, ErrorMessage = "{0} must be at least {2} characters long.", MinimumLength = 20)]
        public string BugDescription { get; set; }
        [Display(Name = "Build or Firmware number")]
        public string BuildNumber { get; set; }
    }
}
