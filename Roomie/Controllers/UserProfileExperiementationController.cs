using Roomie.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Roomie.Controllers
{
    public class UserProfileExperiementationController : Controller
    {
        private RoomieEntities db = new RoomieEntities();
        // GET: UserProfileExperiementation
        public ActionResult Index()
        {
            var userProfiles = db.UserProfiles;
            return View(userProfiles.ToList());
        }
    }
}