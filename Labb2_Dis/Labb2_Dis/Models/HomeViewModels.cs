using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Labb2_Dis.Models
{
    public class UserViewModel
    {

        public string Username { get; set; }
        public string email { get; set; }
        public DateTime lastLogin { get; set; }
        public int NoOfLoginsThisMOnth { get; set; }
        public int NoOfUnreadMessages { get; set; }
    }
}