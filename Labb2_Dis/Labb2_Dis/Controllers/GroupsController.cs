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
    public class GroupsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Groups
        public ActionResult Index()
        {
            //Lazy loading
            ApplicationUser currentUser = db.Users.Find(User.Identity.GetUserId());

            //Eager load Memeberlist in groups - Whole list is beign used
            List<Group> JList = db.Groups.Include(m => m.MemberList).ToList().Intersect(db.Groups.ToList().Where(group => group.MemberList.Any(user => user.Id == currentUser.Id))).ToList();
            List<Group> AList = db.Groups.ToList().Except(JList).ToList();
            Debug.WriteLine(AList.Count);
            Debug.WriteLine(JList.Count);
            List<GroupViewModel> joinedList = new List<GroupViewModel>();
            List<GroupViewModel> avaiableList = new List<GroupViewModel>();

            foreach (var item in JList)
            {
                GroupViewModel group = new GroupViewModel(item.GroupId, item.GroupName);
                joinedList.Add(group);
            }

            foreach (var item in AList)
            {
                GroupViewModel group = new GroupViewModel(item.GroupId, item.GroupName);
                avaiableList.Add(group);
            }

            GroupListViewModel GroupList = new GroupListViewModel(joinedList, avaiableList);
            return View(GroupList);
        }

        // GET: Groups/Send/5
        public ActionResult Send(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group group = db.Groups.Find(id);
            if (group == null)
            {
                return HttpNotFound();
            }
            //Lazy load - No navigation properties are beign used
            List<ApplicationUser> UserList = group.MemberList.ToList();

            GroupAndMessageViewModel groupAndMessage = new GroupAndMessageViewModel(new GroupViewModel(group.GroupId, group.GroupName));
            Debug.WriteLine(groupAndMessage.Group.GroupName);
            return View(groupAndMessage);
        }


        // POST: Groups/Send
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Send(GroupAndMessageViewModel model)
        {
            Debug.WriteLine(ModelState.IsValid);
            Debug.WriteLine(model.Group.GroupName);
            
            if (ModelState.IsValid)
            {
                //Lazy loading user - No navigation properties is used
                ApplicationUser currentUser = db.Users.Find(User.Identity.GetUserId());
                //Lazy loading group - A single navigation property is used
                Group Currentgroup = db.Groups.Find(model.Group.GroupId);
                
                Message msg = new Message();
                msg.From = currentUser.UserName;
                msg.Content = model.Message.Content;
                msg.Title = model.Message.Title;

                for (int i=0; i<Currentgroup.MemberList.Count;i++)
                {
                    if(Currentgroup.MemberList.ElementAt(i).UserName != currentUser.UserName)
                    {
                        msg.To = Currentgroup.MemberList.ElementAt(i);
                        Currentgroup.MemberList.ElementAt(i).NoOfUnreadMessages++;
                        db.Messages.Add(msg);
                    }
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

        // GET: Groups/Join/5
        public ActionResult Join(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Lazy load
            Group group = db.Groups.Find(id);
            //Explicit load group - Navigation property is used belowe
            db.Groups.Where(m => m.GroupId == id).Load();
            if (group == null)
            {
                return HttpNotFound();
            }
            //Lazy loading user - Not using navigation properties
            ApplicationUser currentUser = db.Users.Find(User.Identity.GetUserId());

            if (!group.MemberList.Contains(currentUser))
            {
                group.MemberList.Add(currentUser);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        // GET: Groups/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Groups/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "GroupId,GroupName")] Group group)
        {
            if (ModelState.IsValid)
            {
                //Lazy load user - No navigation properties is used
                ApplicationUser currentUser = db.Users.Find(User.Identity.GetUserId());
                Debug.WriteLine(currentUser.UserName);
                group.MemberList.Add(currentUser);
                db.Groups.Add(group);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(group);
        }

        // GET: Groups/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group group = db.Groups.Find(id);
            if (group == null)
            {
                return HttpNotFound();
            }
            return View(group);
        }

        // POST: Groups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "GroupId,GroupName")] Group group)
        {
            if (ModelState.IsValid)
            {
                db.Entry(group).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(group);
        }

       
        //GET: Groups/Leave/3
        public ActionResult Leave(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Explicit loading
            string Uid = User.Identity.GetUserId();
            ApplicationUser currentUser = db.Users.Include(m => m.GroupList).First(u => u.Id == Uid);
            

            //Find group with id - Eager loading memberlist since it will be iterated
            Group group = db.Groups.Include(g => g.MemberList).FirstOrDefault(h => h.GroupId == id);

            //Iterate all groups from groups that user is member of
            foreach(var g in currentUser.GroupList)
            {
                //Match and remove user from database
                if(group.GroupId == id)
                {
                    currentUser.GroupList.Remove(group);
                    group.MemberList.Remove(currentUser);                                                     
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            //If user is not part of group with id
            return HttpNotFound();
        }
    }
}
