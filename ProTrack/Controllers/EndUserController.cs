using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ProTrack.Controllers
{
    public class EndUserController : Controller
    {
        public IActionResult Entry()
        {
            return View();
        }
    }
}