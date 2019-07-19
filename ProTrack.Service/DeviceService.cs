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
    public class DeviceService : IDevice
    {
        private readonly ApplicationDbContext _context;

        public DeviceService(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task Create(Device device)
        {
            throw new NotImplementedException();
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Device> GetAll()
        {
            return _context.Devices
                .Include(d => d.Manufacturer)
                .Include(d => d.Product);

            
        }

        public Device GetById(int id)
        {
            var device = _context.Devices.Where(d => d.Id == id)
                .Include(d => d.Manufacturer)
                .Include(d => d.Product)
                .FirstOrDefault();

            return device;
        }
    }
}
