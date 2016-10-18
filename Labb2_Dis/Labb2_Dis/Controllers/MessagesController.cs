using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Labb2_Dis.Models;
using Microsoft.AspNet.Identity;
using System.Diagnostics;

namespace Labb2_Dis.Controllers
{
    [Authorize]
    public class MessagesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Messages
        public ActionResult Index()
        {
            var currentUser = db.Users.Find(User.Identity.GetUserId());

            return View(MessageViewModel.GetMessageViewModelList(db.Messages.ToList().Where(
            todo => todo.To == currentUser)));
        }

        // GET: Messages/MessagesFromUser/5
        public ActionResult MessagesFromUser(string username)
        {
            var currentUser = db.Users.Find(User.Identity.GetUserId());
             
            return View(MessageViewModel.GetMessageViewModelList(db.Messages.ToList().Where(
            todo => todo.To == currentUser && todo.From.Equals(username))));
        }

        // GET: Messages/ShowMessage/5
        public ActionResult ShowMessage(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message message = db.Messages.Find(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            return View(message.GetViewModel());
        }

        // GET: Messages/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Messages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "To, Title, Content")] MessageViewModel model)
        {
          
            Message message = new Message();
            if (ModelState.IsValid)
            {

                var SendToUser = db.Users.FirstOrDefault(user => user.UserName == model.To);
                var currentUser = db.Users.Find(User.Identity.GetUserId());
                Console.Write("UserName: " + SendToUser.UserName);
                message.To = SendToUser;
                message.From = currentUser.UserName;
                message.Title = model.Title;
                message.Content = model.Content;
                message.Date = DateTime.Now;
                message.IsRemoved = false;
                message.isRead = false;

                db.Messages.Add(message);
                db.SaveChanges();

                Message SentMessage = db.Messages.ToList().Where(m => m.To == SendToUser && m.From.Equals(currentUser.UserName)).LastOrDefault();
               
                return View("SendReceipt", SentMessage.GetViewModel());
            }

            return View(message);
        }

        // GET: Messages/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message message = db.Messages.Find(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            return View(message);
        }

        // POST: Messages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MessageId,isRead,From,Date,IsRemoved,Content,Title")] Message message)
        {
            if (ModelState.IsValid)
            {
                var currentUser = db.Users.Find(User.Identity.GetUserId());
                db.Entry(message).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(message);
        }

        // GET: Messages/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message message = db.Messages.Find(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            return View(message);
        }

        // POST: Messages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Message message = db.Messages.Find(id);
            db.Messages.Remove(message);
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
