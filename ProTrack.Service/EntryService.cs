using ProTrack.Data;
using ProTrack.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProTrack.Service
{
    public class EntryService : IEntry
    {
        private readonly ApplicationDbContext context;

        public EntryService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task Add(EndUserEntry entry)
        {
            context.Add(entry);
            await context.SaveChangesAsync();
        }
    }
}
