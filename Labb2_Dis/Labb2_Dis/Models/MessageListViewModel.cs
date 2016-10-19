using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Labb2_Dis.Models
{
    public class MessageListViewModel
    {
        public List<MessageViewModel> List { get; set; }

        public MessageListViewModel() {  }

        public MessageListViewModel(List<MessageViewModel> list)
        {
            foreach(var item in list)
            {
                item.IsChecked = false;
            }

            this.List = list;
        }
    }
}