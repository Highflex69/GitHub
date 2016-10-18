using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Labb2_Dis.Models
{
    public class UserListAndMessageViewModel
    {
        public IEnumerable<SelectListItem> UserList { get; set; }
        public MessageViewModel Message { get; set; }

        public UserListAndMessageViewModel() { }
        public UserListAndMessageViewModel(List<UserViewModel> userList, MessageViewModel message)
        {
            this.UserList = GetAllItemToList(userList);
            this.Message = message;
        }

        public List<SelectListItem> GetAllItemToList(List<UserViewModel> userList)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            foreach(var usr in userList)
            {
                list.Add(new SelectListItem
                {
                    Text = usr.Username,
                    Value = usr.Username
                });
            }
            return list;
        }
    }
}