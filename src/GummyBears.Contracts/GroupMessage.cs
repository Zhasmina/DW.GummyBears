using System;
using System.ComponentModel.DataAnnotations;
namespace GummyBears.Contracts
{
    public class GroupMessage
    {
        public int GroupId { get; set; }

        public int UserId { get; set; }

        [Required]
        public string Message { get; set; }

        [Required]
        public DateTime SendDate { get; set; }
    }
}
