﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Roomie.Models;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity;

namespace Roomie.Controllers
{
    [Authorize]
    public class MessageBoardsController : Controller
    {
        private RoomieEntities db = new RoomieEntities();
        public async Task<ActionResult> MessagesForUser(string id)
        {
            var userId = User.Identity.GetUserId();
            var messageBoards = db.MessageBoards.Where(mb => (mb.SenderID == id && mb.RecieverID==userId)|| (mb.RecieverID == id && mb.SenderID==userId)).OrderBy(m => m.SingleMessage.TimeOfMessage);
            ViewBag.recieverID = id;
            return View(await messageBoards.ToListAsync());
        }
        // GET: MessageBoards
        public async Task<ActionResult> Index()
        {
            var userId = User.Identity.GetUserId();
            var messageBoards = db.MessageBoards.Where(mb=>mb.RecieverID == userId).OrderBy(m=>m.SingleMessage.TimeOfMessage);
            return View(await messageBoards.ToListAsync());
        }



        // GET: MessageBoards/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MessageBoard messageBoard = await db.MessageBoards.FindAsync(id);
            if (messageBoard == null)
            {
                return HttpNotFound();
            }
            return View(messageBoard);
        }

        // GET: MessageBoards/Create
        public ActionResult Create(string id)
        {
            
            
            var userId = User.Identity.GetUserId();
            var linker = db.ProfileLinkers.First(pl => (pl.LinkedProfile == userId && pl.UserLinkedId == id) || (pl.LinkedProfile == id && pl.UserLinkedId == userId));
            var message = new MessageCreateViewModel()
            {
                
                RecieverID=id,
                LinkerId=linker.ID
            };
            return PartialView(message);
        }

        // POST: MessageBoards/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(MessageCreateViewModel messageVM)
        {
            

            if (ModelState.IsValid)
            {
                var message = new SingleMessage { id=db.SingleMessages.Count(),MessageText = messageVM.MessageText, TimeOfMessage = DateTime.Now };

                var userId = User.Identity.GetUserId();
                var messageBoard = new MessageBoard { MessageID=db.MessageBoards.Count(),SingleMessage = message, RecieverID = messageVM.RecieverID,SenderID=userId,ProfileLinkerID=messageVM.LinkerId };
                db.MessageBoards.Add(messageBoard);
                await db.SaveChangesAsync();
                return RedirectToAction("MessagesForUser","MessageBoards",new { id=messageVM.RecieverID});
            }
            return View();
        }

        // GET: MessageBoards/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MessageBoard messageBoard = await db.MessageBoards.FindAsync(id);
            if (messageBoard == null)
            {
                return HttpNotFound();
            }
            ViewBag.MessageID = new SelectList(db.SingleMessages, "id", "MessageText", messageBoard.MessageID);
            ViewBag.ProfileLinkerID = new SelectList(db.ProfileLinkers, "ID", "UserLinkedId", messageBoard.ProfileLinkerID);
            ViewBag.RecieverID = new SelectList(db.UserProfiles, "Id", "FirstName", messageBoard.RecieverID);
            ViewBag.SenderID = new SelectList(db.UserProfiles, "Id", "FirstName", messageBoard.SenderID);
            return View(messageBoard);
        }

        // POST: MessageBoards/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "SenderID,RecieverID,ProfileLinkerID,MessageID")] MessageBoard messageBoard)
        {
            if (ModelState.IsValid)
            {
                db.Entry(messageBoard).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.MessageID = new SelectList(db.SingleMessages, "id", "MessageText", messageBoard.MessageID);
            ViewBag.ProfileLinkerID = new SelectList(db.ProfileLinkers, "ID", "UserLinkedId", messageBoard.ProfileLinkerID);
            ViewBag.RecieverID = new SelectList(db.UserProfiles, "Id", "FirstName", messageBoard.RecieverID);
            ViewBag.SenderID = new SelectList(db.UserProfiles, "Id", "FirstName", messageBoard.SenderID);
            return View(messageBoard);
        }

        // GET: MessageBoards/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MessageBoard messageBoard = await db.MessageBoards.FindAsync(id);
            if (messageBoard == null)
            {
                return HttpNotFound();
            }
            return View(messageBoard);
        }

        // POST: MessageBoards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            MessageBoard messageBoard = await db.MessageBoards.FindAsync(id);
            db.MessageBoards.Remove(messageBoard);
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
