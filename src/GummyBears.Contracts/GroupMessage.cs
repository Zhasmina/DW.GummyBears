using System;
using System.ComponentModel.DataAnnotations;
namespace GummyBears.Contracts
{
    public class GroupMessage
    {
        public int GroupId { get; set; }

        public int UserId { get; set; }

        [Required(ErrorMessage ="Message is empty")]
        public string Message { get; set; }

        [Required(ErrorMessage = "Send date not populated")]
        public DateTime SendDate { get; set; }
    }
}
