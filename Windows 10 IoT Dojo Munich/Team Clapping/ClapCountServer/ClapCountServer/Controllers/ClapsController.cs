using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClapCountServer.Controllers
{
    public class ClapsController : Controller
    {
        // GET: Claps
        public ActionResult Index()
        {
            return View();
        }
    }
}