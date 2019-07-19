using ProTrack.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProTrack.Data
{
    public interface IEntry
    {
        Task Add(EndUserEntry entry);
    }
}
