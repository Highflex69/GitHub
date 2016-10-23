using Labb2_Dis.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Labb2_Dis.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index(UserViewModel model)
        {
            if(ModelState.IsValid)
            {
                
                var currentUser = db.Users.Find(User.Identity.GetUserId());
                //Explicit loading messages for message counting
                db.Messages.Where(m => m.To.Id == currentUser.Id).Load();

                int MessageCount = 0;
                foreach(var message in currentUser.MessageList)
                {
                    if(!message.isRead && !message.IsRemoved)
                    {
                        MessageCount++;
                    }
                }
               
                model.Username = currentUser.UserName;
                model.email = currentUser.Email;
                model.lastLogin = currentUser.LastLogin;
                model.NoOfLoginsThisMOnth = currentUser.NoOfLoginThisMonth;
                model.NoOfUnreadMessages = MessageCount;
                return View(model);
            }
            return View(model);
        }
    }
}