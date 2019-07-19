using ProTrack.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProTrack.Data
{
    public class DataSeeder
    {
        private readonly ApplicationDbContext context;

        public DataSeeder(ApplicationDbContext context)
        {
            this.context = context;
        }

        public Task SeedManufacturers()
        {
            string[] manufacturers =
            {
                "Pakedge","Apple","Araknis","Asus","Belkin","Cisco","Control4","Dell","DLink","EnGenius","Fortinet","Google","HP","IBM","Linksys","Luxul","Meraki","Middle Atlantic","Mikrotik","Motorola","Netgear","Planet","Ruckus","SonicWall","TP-Link","TrendNet","Ubiquiti","WattBox","Zyxel"

            };
            List<string> mfgList = new List<string>(manufacturers);

            foreach (var mfg in mfgList)
            {
                var mfgs = new Manufacturer
            {
                ManufacturerName = mfg
            };
                context.Add(mfgs);
                
            }

            context.SaveChangesAsync();
            return Task.CompletedTask;
        }
    }
}
