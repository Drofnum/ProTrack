using ProTrack.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProTrack.Data
{
    public interface IDevice
    {
        Device GetById(int id);

        IEnumerable<Device> GetAll();

        Task Create(Device device);
        Task Delete(int id);
    }
}
