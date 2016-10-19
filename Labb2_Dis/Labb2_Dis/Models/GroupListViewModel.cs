using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Labb2_Dis.Models
{
    public class GroupListViewModel
    {
        public List<GroupViewModel> JoinedGroup { get; set; }
        public List<GroupViewModel> AvailableGroup { get; set; }
        public GroupListViewModel() { }

        public GroupListViewModel(List<GroupViewModel> joinedlist, List<GroupViewModel> availablelist)
        {
           
            JoinedGroup = joinedlist;
            AvailableGroup = availablelist;
        }
    }
}