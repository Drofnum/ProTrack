using Microsoft.EntityFrameworkCore;
using ProTrack.Data;
using ProTrack.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProTrack.Service
{
    public class LocationService : ILocation
    {
        private readonly ApplicationDbContext _context;

        public LocationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task Create(Location location)
        {
            throw new NotImplementedException();
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Location> GetAll()
        {
            throw new NotImplementedException();
        }

        public Location GetById(int? id)
        {
            return _context.Locations.Where(l => l.Id == id)
            .Include(l => l.Devices)
            .FirstOrDefault();
        }
    }
}
