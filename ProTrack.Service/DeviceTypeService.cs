using ProTrack.Data;
using ProTrack.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProTrack.Service
{
    class DeviceTypeService : IDeviceType
    {
        private readonly ApplicationDbContext _context;

        public DeviceTypeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task Create(DeviceType deviceType)
        {
            throw new NotImplementedException();
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DeviceType> GetAll()
        {
            throw new NotImplementedException();
        }

        public DeviceType GetById(int? id)
        {
            throw new NotImplementedException();
        }
    }
}
