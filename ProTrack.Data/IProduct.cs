using ProTrack.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProTrack.Data
{
    public interface IProduct
    {
        Product GetById(int? id);

        IEnumerable<Product> GetAll();

        Task Create(Product product);
        Task Delete(int id);
    }
}
