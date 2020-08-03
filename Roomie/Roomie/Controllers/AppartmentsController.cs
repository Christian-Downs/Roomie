using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Roomie.Models;
using System.IO;
using Microsoft.AspNet.Identity;

namespace Roomie.Controllers
{
    [Authorize]
    public class AppartmentsController : Controller
    {
        private RoomieEntities db = new RoomieEntities();


        // GET: Appartments
        public async Task<ActionResult> MyAppartments()
        {
            var userId = User.Identity.GetUserId();
            var appartmentOwner = db.AppartmentOwners.Find(userId);
            var appartments = appartmentOwner.Appartment;
            return View(appartments);
        }
        public async Task<ActionResult> BookAppointment(int id)
        {
            Appartment appartment = await db.Appartments.FindAsync(id);
            return View(appartment);
        }

        // GET: Appartments
        public async Task<ActionResult> Index()
        {
            var appartments = db.Appartments.Include(a => a.Photo);
            return View(await appartments.ToListAsync());
        }

        // GET: Appartments/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appartment appartment = await db.Appartments.FindAsync(id);
            if (appartment == null)
            {
                return HttpNotFound();
            }
            return View(appartment);
        }

        [Authorize(Roles ="Owner")]
        // GET: Appartments/Create
        public ActionResult Create()
        {
            ViewBag.PhotoID = new SelectList(db.Photos, "ID", "ImageLocation");
            return View();
        }

        // POST: Appartments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "RentCost,Description,PhotoID,Street,City,Country,ZipCode,State")] Appartment appartment, HttpPostedFileBase photoInput)
        {
            bool isValid = ModelState.IsValid;
            if(isValid && photoInput!=null && photoInput.ContentLength>0 )
            {
                if(!photoInput.ContentType.StartsWith("image/"))
                {
                    isValid = false;
                    ModelState.AddModelError("PhotoID", "Photo much be an image");
                }
                
            }

            if (isValid)
            {

                if (photoInput != null && photoInput.ContentLength > 0)
                {
                    var imageFolder = Server.MapPath("/UploadedImages");
                    var imageFileName = DateTime.Now.ToString("yyyy-MM-dd_HHmmss_fff") + "_" + photoInput.FileName;
                    var imagePath = Path.Combine(imageFolder, imageFileName);

                    photoInput.SaveAs(imagePath);
                    var photo = new Photo()
                    {
                        ImageLocation = imageFileName

                    };

                    appartment.Photo = photo; 
                }
                var userId = User.Identity.GetUserId();
                var appartmentOwner = db.AppartmentOwners.Find(userId);
                appartmentOwner.Appartment = appartment;
                db.Entry(appartmentOwner).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.PhotoID = new SelectList(db.Photos, "ID", "ImageLocation", appartment.PhotoID);
            return View(appartment);
        }

        // GET: Appartments/Edit/5
        public async Task<ActionResult> Edit()
        {
            var userId = User.Identity.GetUserId();
            var appartmentOwner = db.AppartmentOwners.Find(userId);
            var appartments = appartmentOwner.Appartment;
            return View(appartments);
        }

        // POST: Appartments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,RentCost,Description,PhotoID,Street,City,Country")] Appartment appartment, HttpPostedFileBase photoInput)
        {
            bool isValid = ModelState.IsValid;

            if (isValid && photoInput != null && photoInput.ContentLength > 0)
            {
                if (!photoInput.ContentType.StartsWith("image/"))
                {
                    isValid = false;
                    ModelState.AddModelError("PhotoID", "Photo much be an image");
                }

            }

            if (isValid)
            {

                if (photoInput != null && photoInput.ContentLength > 0)
                {
                    var imageFolder = Server.MapPath("/UploadedImages");
                    var imageFileName = DateTime.Now.ToString("yyyy-MM-dd_HHmmss_fff") + "_" + photoInput.FileName;
                    var imagePath = Path.Combine(imageFolder, imageFileName);

                    photoInput.SaveAs(imagePath);
                    var photo = new Photo()
                    {
                        ImageLocation = imageFileName

                    };

                    appartment.Photo = photo;
                }

                db.Entry(appartment).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.PhotoID = new SelectList(db.Photos, "ID", "ImageLocation", appartment.PhotoID);
            return View(appartment);
        }

        // GET: Appartments/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appartment appartment = await db.Appartments.FindAsync(id);
            if (appartment == null)
            {
                return HttpNotFound();
            }
            return View(appartment);
        }

        // POST: Appartments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Appartment appartment = await db.Appartments.FindAsync(id);
            db.Appartments.Remove(appartment);
            await db.SaveChangesAsync();
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
