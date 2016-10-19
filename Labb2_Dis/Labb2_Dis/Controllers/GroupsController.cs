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
            ApplicationUser currentUser = db.Users.Find(User.Identity.GetUserId());

            List<Group> JList = db.Groups.ToList().Intersect(db.Groups.ToList().Where(group => group.MemberList.Any(user => user.Id == currentUser.Id))).ToList();
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
                ApplicationUser currentUser = db.Users.Find(User.Identity.GetUserId());
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
            Group group = db.Groups.Find(id);
            if (group == null)
            {
                return HttpNotFound();
            }
            ApplicationUser currentUser = db.Users.Find(User.Identity.GetUserId());
            if(!group.MemberList.Contains(currentUser))
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
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "GroupId,GroupName")] Group group)
        {
            if (ModelState.IsValid)
            {
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

        // GET: Groups/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: Groups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Group group = db.Groups.Find(id);
            db.Groups.Remove(group);
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
