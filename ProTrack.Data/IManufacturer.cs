using ProTrack.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProTrack.Data
{
    public interface IManufacturer
    {
        Manufacturer GetById(int? id);

        IEnumerable<Manufacturer> GetAll();

        Task Create(Manufacturer manufacturer);
        Task Delete(int id);
    }
}
