using System.ComponentModel.DataAnnotations;

namespace ProTrack.Models.Betas
{
    public class SubmitFeedbackListingModel
    {
        public int Id { get; set; }
        [Display(Name = "Project Name")]
        [Required]
        public string ProjectName { get; set; }
        [Display(Name = "Feedback")]
        [Required]
        [StringLength(255, ErrorMessage = "{0} must be at least {2} characters long.", MinimumLength = 15)]
        public string Summary { get; set; }
    }
}
