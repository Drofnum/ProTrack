using ProTrack.Data;
using ProTrack.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProTrack.Service
{
    class ManufacturerService : IManufacturer
    {
        private readonly ApplicationDbContext _context;

        public ManufacturerService(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task Create(Manufacturer manufacturer)
        {
            throw new NotImplementedException();
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Manufacturer> GetAll()
        {
            throw new NotImplementedException();
        }

        public Manufacturer GetById(int? id)
        {
            throw new NotImplementedException();
        }
    }
}
