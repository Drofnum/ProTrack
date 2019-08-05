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

        public async Task Create(Device device)
        {
            _context.Add(device);
            await _context.SaveChangesAsync();
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Device> GetAll()
        {
            return _context.Devices
                .Include(d => d.Product);

            
        }

        public Device GetById(int? id)
        {
            return _context.Devices.Where(d => d.Id == id)
                .Include(d => d.Location)
                .Include(d => d.Product)
                .ThenInclude(p => p.Manufacturer)
                .FirstOrDefault();
        }

        public IEnumerable<Device> GetDeviceByName(string searchQuery)
        {
            var query = searchQuery.ToLower();

            return _context.Devices
                .Include(d => d.Product)
                .ThenInclude(p => p.Manufacturer)
                .Include(d => d.Location)
                .Where(d =>
                d.Product.ProductName.ToLower().Contains(query)
                || d.Product.Manufacturer.ManufacturerName.ToLower().Contains(query));
                
        }
    }
}
