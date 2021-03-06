﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Labb2_Dis.Models
{
    public class GroupAndMessageViewModel
    {
        [Key]
        public GroupViewModel Group { get; set; }
        public MessageViewModel Message { get; set; }
        public GroupAndMessageViewModel() { }
        public GroupAndMessageViewModel(GroupViewModel group)
        {
            this.Group = group;
            this.Message = new MessageViewModel();
        }
    }
}