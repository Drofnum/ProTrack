using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProTrack.Models.Betas
{
    public class ApplicantIndexModel
    {
        public IEnumerable<ApplicantListingModel> ApplicantList { get; set; }
        public List<SelectListItem> BetasList { get; set; }
        public string BetaOpportunity { get; set; }
    }
}
