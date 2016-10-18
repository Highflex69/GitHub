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

        public UserViewModel() { }

        public UserViewModel(string username, string email, DateTime lastLogin, int noOfLoginsThisMonth, int noOfUnreadMessages)
        {
            this.Username = username;
            this.email = email;
            this.lastLogin = lastLogin;
            this.NoOfLoginsThisMOnth = noOfLoginsThisMonth;
            this.NoOfUnreadMessages = noOfUnreadMessages;
        }

        public static List<UserViewModel> GetAllUserViewModelList(IEnumerable<ApplicationUser> list )
        {
            List<UserViewModel> UserViewModelList = new List<UserViewModel>();

            
            foreach(var usr in list)
            {
                UserViewModelList.Add(usr.getUserViewModel());
            }

            return UserViewModelList;
        }
    }

    
}