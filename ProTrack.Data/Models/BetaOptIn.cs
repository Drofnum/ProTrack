using System;
using System.Collections.Generic;
using System.Text;

namespace ProTrack.Data.Models
{
    public class BetaOptIn
    {
        public int Id { get; set; }
        public BetaOpportunity BetaOpportunity { get; set; }
        public ApplicationUser User { get; set; }
    }
}
