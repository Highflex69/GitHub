using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Labb2_Dis.Models
{
    public class Message
    {
        [Key]
        public int MessageId { get; set; }
        public bool isRead { get; set; }
        public string To { get; set; }
        public string From { get; set; }
        public DateTime Date { get; set; }
        public bool IsRemoved { get; set; }
    }
}