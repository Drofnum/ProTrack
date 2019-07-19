using ProTrack.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProTrack.Data
{
    public interface IDeviceType
    {
        DeviceType GetById(int? id);

        IEnumerable<DeviceType> GetAll();

        Task Create(DeviceType deviceType);
        Task Delete(int id);
    }
}
