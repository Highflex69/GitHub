using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Labb2_Dis.Models
{
    public class Group
    {
        [Key]
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        //public string Admin { get; set; }
        public virtual ICollection<ApplicationUser> MemberList { get; set; }
        
        public Group() { MemberList = new List<ApplicationUser>(); }


    }
}