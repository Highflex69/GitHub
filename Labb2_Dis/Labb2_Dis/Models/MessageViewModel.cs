using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Labb2_Dis.Models
{
    public class MessageViewModel
    {
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "To")]
        public string To { get; set; }

        [Required]  
        [Display(Name ="Title")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Content")]
        public string Content { get; set; }

        [Display(Name = "Date")]
        public DateTime Date { get; set; }

        [Display(Name = "From")]
        public string From { get; set; }

        [Display(Name = "IsRead")]
        public bool isRead { get; set; }

        [Display(Name = "IsRemoved")]
        public bool IsRemoved { get; set; }

        [Display(Name = "IsChecked")]
        public bool IsChecked { get; set; }

        public MessageViewModel()
        {

        }

        public MessageViewModel(int id, bool isRead, string to, string from, DateTime Date, bool isRemoved, string content, string title)
        {
            this.Id = id;
            this.isRead = isRead;
            this.To = to;
            this.From = from;
            this.Date = Date;
            this.IsRemoved = isRemoved;
            this.Content = content;
            this.Title = title;

        }

        public static IEnumerable<MessageViewModel> GetMessageViewModelList(IEnumerable<Message> list)
        {
            List<MessageViewModel> NewList = new List<MessageViewModel>();

            foreach (var msg in list)
            {
                NewList.Add(msg.GetViewModel());
            }

            return NewList;
        } 
    }
}