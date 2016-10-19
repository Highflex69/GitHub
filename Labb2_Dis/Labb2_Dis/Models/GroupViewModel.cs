
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Labb2_Dis.Models
{
    public class GroupViewModel
    {
        [Key]
        [Required]
        [Display(Name = "GroupId")]
        public int GroupId { get; set; }

        [Required]
        [Display(Name = "GroupName")]
        public string GroupName { get; set; }

        public GroupViewModel() { }

        public GroupViewModel(int id, string name)
        {
            this.GroupId = id;
            this.GroupName = name;
        }

    }
}