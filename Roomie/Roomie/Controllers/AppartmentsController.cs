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

namespace Roomie.Controllers
{
    [Authorize]
    public class AppartmentsController : Controller
    {
        private RoomieEntities db = new RoomieEntities();

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
        public async Task<ActionResult> Create([Bind(Include = "ID,RentCost,Description,PhotoID,Street,City,Country")] Appartment appartment)
        {
            if (ModelState.IsValid)
            {
                db.Appartments.Add(appartment);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.PhotoID = new SelectList(db.Photos, "ID", "ImageLocation", appartment.PhotoID);
            return View(appartment);
        }

        // GET: Appartments/Edit/5
        public async Task<ActionResult> Edit(int? id)
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
            ViewBag.PhotoID = new SelectList(db.Photos, "ID", "ImageLocation", appartment.PhotoID);
            return View(appartment);
        }

        // POST: Appartments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,RentCost,Description,PhotoID,Street,City,Country")] Appartment appartment)
        {
            if (ModelState.IsValid)
            {
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
