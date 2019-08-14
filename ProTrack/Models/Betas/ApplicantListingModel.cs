using Microsoft.AspNetCore.Mvc.Rendering;
using ProTrack.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProTrack.Models.Betas
{
    public class ApplicantListingModel
    {
        public int Id { get; set; }
        public string ProjectName { get; set; }
        public string Description { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public int Accepted { get; set; }
        public int BetaOpportunityId { get; set; }

    }
}
