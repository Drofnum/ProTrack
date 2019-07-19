using ProTrack.Data;
using ProTrack.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProTrack.Service
{
    public class ApplicationUserService : IApplicationUser
    {
        private readonly ApplicationDbContext context;

        public ApplicationUserService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IEnumerable<ApplicationUser> GetAll()
        {
            return context.ApplicationUser;
        }

        public ApplicationUser GetById(string id)
        {
            return GetAll().FirstOrDefault(user => user.Id == id);
        }
    }
}
