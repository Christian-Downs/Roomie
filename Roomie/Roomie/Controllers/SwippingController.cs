using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Roomie.Controllers
{
    [Authorize]
    public class SwippingController : Controller
    {
        // GET: Swipping
        public ActionResult Index()
        {
            return View();
        }
    }
}