using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProTrack.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProTrack.Data
{
    public class DataSeeder
    {
        private readonly ApplicationDbContext context;

        public DataSeeder(ApplicationDbContext context)
        {
            this.context = context;
        }

    }
}
