using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClapCountServer.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About(int count)
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpGet]
        public ActionResult Claps(int id)
        {
            ViewBag.Count = id;
            var hub = GlobalHost.ConnectionManager.GetHubContext<ClapHub>();
            hub.Clients.All.NewClaps(id);
            return View();
        }
    }
}