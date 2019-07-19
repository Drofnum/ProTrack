using ProTrack.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProTrack.Data
{
    public interface IApplicationUser
    {
        ApplicationUser GetById(string id);
        IEnumerable<ApplicationUser> GetAll();
    }
}
