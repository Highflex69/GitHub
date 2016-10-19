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
        public virtual ApplicationUser To { get; set; }
        public string From { get; set; }
        public DateTime Date { get; set; }
        public bool IsRemoved { get; set; }
        public string Content { get; set; }
        public string Title { get; set; }

        public Message()
        {
            this.isRead = false;
            this.IsRemoved = false;
            this.MessageId = -1;
            this.Title = "";
            this.To = null;
            this.From = "";
            this.Date = DateTime.Now;
            this.Content = "";
        }
        public MessageViewModel GetViewModel()
        {
            return new MessageViewModel(this.MessageId, this.isRead, this.To.UserName, this.From, this.Date, this.IsRemoved, this.Content, this.Title);
        }
    }
}