using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PcStoreWebsite.Controllers
{
    public class GuestController : Controller
    {
        // GET: Guest
        public ActionResult GHome()
        {
            return View();
        }
    }
}