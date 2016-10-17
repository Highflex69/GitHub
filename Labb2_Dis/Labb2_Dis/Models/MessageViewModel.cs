using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Labb2_Dis.Models
{
    
    public class MessageViewModel
    {
        [Required]
        [Display(Name = "To")]
        public string To { get; set; }

        [Required]  
        [Display(Name ="Title")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Content")]
        public string Content { get; set; }

        [Display(Name = "Time")]
        public DateTime SendTime { get; set; }
    
    }
}