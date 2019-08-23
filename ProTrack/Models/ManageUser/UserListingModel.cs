using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProTrack.Models.ManageUser
{
    public class UserListingModel
    {
        public UserListingModel()
        {
            SelectedRoles = new List<string>();
        }

        public string Id { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Display(Name = "Name")]
        public string FullName { get; set; }
        [Display(Name = "Email Address")]
        public string Email { get; set; }
        public string NormalizedEmail { get; set; }
        public bool EmailConfirmed { get; set; }
        [Display(Name = "Is user active?")]
        public bool IsInternal { get; set; }
        public List<SelectListItem> Roles { get; set; }
        public List<string> SelectedRoles { get; set; }
    }
}
