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
            //Lazyload user
            var currentUser = db.Users.Find(User.Identity.GetUserId());
            Debug.WriteLine("in Index GET");

            //Explicit load user messages
            db.Messages.Where(m => m.To.Id == currentUser.Id).Load();

            var UserMessageList = currentUser.MessageList;
            var dbList = UserMessageList;

            IEnumerable<MessageViewModel> list = MessageViewModel.GetMessageViewModelList(dbList);

            return View(list);
        }

        // POST: Messages
        [HttpPost, ActionName("MarkAsRead")]
        [ValidateAntiForgeryToken]
        public ActionResult MarkAsRead(string[] checkbox)
        {
            if (checkbox == null)
            {
 
            }
            else
            {
                //Lazy load used for user and messages - few rows affected
                ApplicationUser currentUser = db.Users.Find(User.Identity.GetUserId());

                foreach (var item in checkbox)
                {
                    int Nr = -1;
                    if (Int32.TryParse(item, out Nr))
                    {
                        Message m = db.Messages.Find(Nr);
                        if (m.To.Id.Equals(User.Identity.GetUserId()))
                        {
                            m.isRead = true;
                            currentUser.NoOfUnreadMessages--;
                        }
                    }
                }
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        // GET: Messages/MessagesFromUser/5
        public ActionResult MessagesFromUser(string username)
        {
            //Lazy load user
            var currentUser = db.Users.Find(User.Identity.GetUserId());

            //Eager loading is used including To - All messages from a certain user is
            var MessageList = db.Messages.Include(m => m.To).ToList().Where(todo => todo.To == currentUser && todo.From.Equals(username));

            return View(MessageViewModel.GetMessageViewModelList(MessageList));
        }

        // GET: Messages/ShowMessage/5
        public ActionResult ShowMessage(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //Lazy loading user
            ApplicationUser CurrentUser = db.Users.Find(User.Identity.GetUserId());
            //Explicit load messages in user - list is going to be iterated
            db.Messages.Where(m => m.To.Id == CurrentUser.Id).Load();


            Message message = db.Messages.Find(id);

            foreach(var m in CurrentUser.MessageList)
            {
                if(m.MessageId == id)
                {
                    m.isRead = true;
                    CurrentUser.NoOfUnreadMessages--;
                    db.SaveChanges();
                    return View(m.GetViewModel());
                }
            }


            return HttpNotFound();
        }

        // GET: Messages/Create
        public ActionResult Create()
        {
            //Eager loading is used - Displaying all available users
            UserListAndMessageViewModel UserListAndMessage = new UserListAndMessageViewModel(UserViewModel.GetAllUserViewModelList(db.Users.ToList()), new MessageViewModel(-1, false, "", "", DateTime.Now, false, "", ""));
            return View(UserListAndMessage);
        }

        // POST: Messages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UserListAndMessageViewModel model)
        {   
            
            Message message = new Message();
            if (ModelState.IsValid)
            {
                Debug.WriteLine(model.Message.Content);
                //Lazy loading user
                var currentUser = db.Users.Find(User.Identity.GetUserId());

                List<Message> sentMessageList = new List<Message>();
                foreach(var item in model.SelectedUserList)
                {
                    //Lazy loading - No navigation properties is used
                    var SendToUser = db.Users.FirstOrDefault(user => user.UserName == item);
                    message.To = SendToUser;
                    message.From = currentUser.UserName;
                    message.Title = model.Message.Title;
                    message.Content = model.Message.Content;
                    SendToUser.NoOfUnreadMessages++;
                    db.Messages.Add(message);
                    db.SaveChanges();            
                }              
               
                foreach(var item in model.SelectedUserList)
                {
                    var SendToUser = db.Users.FirstOrDefault(user => user.UserName == item);
                    //Eager loading message - To field is navigation property
                    Message tmp = db.Messages.Include(m => m.To).ToList().Where(m => m.To == SendToUser && m.From.Equals(currentUser.UserName)).LastOrDefault();
                    Debug.WriteLine("username: " + tmp.To.UserName);
                    sentMessageList.Add(tmp);
                }
                
                return View("SendReceipt", MessageViewModel.GetMessageViewModelList(sentMessageList));
            }
            return HttpNotFound();
        }

        // GET: Messages/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Lazy loading used, only one row in Message table is used
            Message message = db.Messages.Find(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            if(message.To.Id.Equals(User.Identity.GetUserId()))
            {
                message.IsRemoved = true;
                //Lazy loading user - No navigation properties is used
                ApplicationUser currentUser = db.Users.Find(User.Identity.GetUserId());
                message.isRead = true;
                currentUser.NoOfUnreadMessages--;
                db.SaveChanges();
            }
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
