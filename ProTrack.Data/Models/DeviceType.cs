using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ProTrack.Data.Models
{
    public class DeviceType
    {
        public int Id { get; set; }
        public string Type { get; set; }
        [NotMapped]
        public List<SelectListItem> mfgList { get; set; }
    }
}
