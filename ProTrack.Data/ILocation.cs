using ProTrack.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProTrack.Data
{
    public interface ILocation
    {
        Location GetById(int? id);

        IEnumerable<Location> GetAll();

        Task Create(Location location);
        Task Delete(int id);
    }
}
