using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Roomie.Models;

namespace Roomie.Controllers
{
    public class UserProfilesController : Controller
    {
        private RoomieEntities db = new RoomieEntities();



        [Authorize]
        [HttpGet]
        public ActionResult Skip(string id)
        {
            var RedirectUrl=UserProfile.NextProfile(id,db,HttpContext);
            return Redirect(RedirectUrl);
            //var loggedInProfile = User.Identity.GetUserId();
            
            //List<UserProfile> userProfilesById = db.UserProfiles.OrderBy(up => up.Id).Select(up=>up).ToList<UserProfile>();
            ////var currentProfile = Convert.ToString(db.UserProfiles.Where(up => up.Id == id || up.ProfileLinkers.Select(pl => pl.Liked).Equals(false)).Select(cp => cp.Id));
            //for(int i =0; i<userProfilesById.Count;i++)
            //{
            //    if (id == userProfilesById[i].Id)
            //    {
            //        try
            //        {
            //            var redirector = userProfilesById[i + 1].Id;

            //            var url = "https://localhost:44322/UserProfiles/Details/" + redirector;

            //            return Redirect(url);
            //        }
            //        catch
            //        {
            //            return RedirectToAction("Index");
            //        }

            //    }

            //}
            //return RedirectToAction("Index");
        }


        [Authorize]
        [HttpPost]
        public ActionResult Like(string id)
        {
            var userId = User.Identity.GetUserId();
            var logedinProfile = db.UserProfiles.Find(userId);




            var profileLinker = new ProfileLinker
            {
                ID = db.ProfileLinkers.Count(),
                Liked = true,
                Favorited=false,
                LinkedProfile = userId,
                UserLinkedId = id
            };



            db.ProfileLinkers.Add(profileLinker);
            db.SaveChanges();

            var RedirectUrl = UserProfile.NextProfile(id, db, HttpContext);
            return Redirect(RedirectUrl);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Favorite(string id)
        {
            var userId = User.Identity.GetUserId();
            var logedinProfile = db.UserProfiles.Find(userId);




            var profileLinker = new ProfileLinker
            {
                ID = db.ProfileLinkers.Count(),
                Liked = true,
                Favorited = true,
                LinkedProfile = userId,
                UserLinkedId = id
            };



            db.ProfileLinkers.Add(profileLinker);
            db.SaveChanges();


            var linkedUser = db.UserProfiles.Find(id);
            var relativeUrl = Url.Action("Details", "UserProfiles", new { id = id });
            var builder = new UriBuilder(Request.Url.AbsoluteUri) { Path = relativeUrl };
            var absoluteUrl = builder.Uri.ToString();
            string body = $"<div>Congratulations, {linkedUser.FirstName}</div>" +
                $"<div> {logedinProfile.FirstName} has favorited you!</div>" +
                $"<div><a href = \"{absoluteUrl}\">Click here to see their profile!</a></div>";

            MessageSender.SendEmail(linkedUser.EmailAddress, "You've been favorited!", body, MessageSender.BodyType.Html);

            var RedirectUrl = UserProfile.NextProfile(id, db, HttpContext);
            return Redirect(RedirectUrl);
        }

        [Authorize]
        public ActionResult LikedPeople()
        {
            var userId = User.Identity.GetUserId();
            var profileList = db.ProfileLinkers.Where(linker => linker.LinkedProfile==userId).OrderBy(linker=>linker.Favorited).Select(linker=>linker.UserProfile1);

            return View(profileList.ToList());
        }
            
        
        [Authorize]
        // GET: UserProfiles
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var userProfiles = db.UserProfiles.Where(u=>u.Id!=userId);
            return View(userProfiles.ToList());
        }

        // GET: UserProfiles/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserProfile userProfile = db.UserProfiles.Find(id);
            if (userProfile == null)
            {
                return HttpNotFound();
            }

            var userId = User.Identity.GetUserId();
            var linkedUsers = db.ProfileLinkers.Where(pl => pl.LinkedProfile == userId).Select(pl => pl.UserProfile1);
            var avaliableUsers = db.UserProfiles.Where(up => up.Id != userId)
                .Except(linkedUsers);
            var choosenUser = avaliableUsers.OrderBy(up => Guid.NewGuid()).FirstOrDefault();

            return View(userProfile);
        }

        // GET: UserProfiles/Create
        public ActionResult Create()
        {
            ViewBag.AddressID = new SelectList(db.Addresses, "ID", "City");
            ViewBag.PhotoID = new SelectList(db.Photos, "ID", "ImageLocation");
            ViewBag.ProfileLinkerId = new SelectList(db.ProfileLinkers, "ID", "UserLinkedId");
            return View();
        }

        // POST: UserProfiles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FirstName,LastName,PhoneNumber,EmailAddress,Description,PropertyBool,AddressID,ProfileLinkerId,PhotoID")] UserProfile userProfile)
        {
            if (ModelState.IsValid)
            {
                db.UserProfiles.Add(userProfile);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AddressID = new SelectList(db.Addresses, "ID", "City", userProfile.AddressID);
            ViewBag.PhotoID = new SelectList(db.Photos, "ID", "ImageLocation", userProfile.PhotoID);
            return View(userProfile);
        }


        [Authorize]
        // GET: UserProfiles/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserProfile userProfile = db.UserProfiles.Find(id);
            if (userProfile == null)
            {
                return HttpNotFound();
            }
            ViewBag.AddressID = new SelectList(db.Addresses, "ID", "City", userProfile.AddressID);
            ViewBag.PhotoID = new SelectList(db.Photos, "ID", "ImageLocation", userProfile.PhotoID);
            return View(userProfile);
        }

        // POST: UserProfiles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,PhoneNumber,EmailAddress,Description,PropertyBool,AddressID,ProfileLinkerId,PhotoID,City")] UserProfile userProfile)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userProfile).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AddressID = new SelectList(db.Addresses, "ID", "City", userProfile.AddressID);
            ViewBag.PhotoID = new SelectList(db.Photos, "ID", "ImageLocation", userProfile.PhotoID);
            return View(userProfile);
        }

        // GET: UserProfiles/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserProfile userProfile = db.UserProfiles.Find(id);
            if (userProfile == null)
            {
                return HttpNotFound();
            }
            return View(userProfile);
        }

        // POST: UserProfiles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            UserProfile userProfile = db.UserProfiles.Find(id);
            db.UserProfiles.Remove(userProfile);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
