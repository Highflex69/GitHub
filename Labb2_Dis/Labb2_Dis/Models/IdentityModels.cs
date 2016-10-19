﻿using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;

namespace Labb2_Dis.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public DateTime LastLogin {get; set;}
        public DateTime LastLoginReset { get; set; }
        public int NoOfLoginThisMonth {get; set;}
        public int NoOfUnreadMessages {get; set;}
        public virtual ICollection<Message> MessageList { get; set; }
        public virtual ICollection<Group> GroupList { get; set; }
        public ApplicationUser()
        {
            MessageList = new List<Message>();
            GroupList = new List<Group>();
        }

    public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        public UserViewModel getUserViewModel()
        {
            return new UserViewModel(this.UserName, this.Email, this.LastLogin, this.NoOfLoginThisMonth, this.NoOfUnreadMessages);
        }

    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public DbSet<Message> Messages { get; set; }
        public DbSet<Group> Groups { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ApplicationUser>().ToTable("User").Property(p => p.Id).HasColumnName("UserId");
        }
        
    }
}