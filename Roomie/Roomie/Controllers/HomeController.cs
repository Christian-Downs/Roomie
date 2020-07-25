using Microsoft.AspNet.Identity;
using Roomie.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Roomie.Controllers
{
    public class HomeController : Controller 
    {
        private RoomieEntities db = new RoomieEntities();
        [Authorize]
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var userProfiles = db.UserProfiles.Where(u => u.Id != userId);
            return View(userProfiles.ToList());
        }
        [Authorize]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        [Authorize]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}