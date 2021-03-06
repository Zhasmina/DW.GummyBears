﻿using System;
using System.ComponentModel.DataAnnotations;
namespace GummyBears.Contracts
{
    public class GroupMessage
    {
        public int Id { get; set; }

        public int GroupId { get; set; }

        public int UserId { get; set; }

        public string Username { get; set; }

        [Required(ErrorMessage ="Message is empty")]
        public string Message { get; set; }

        [Required(ErrorMessage = "Send date not populated")]
        public DateTime SendDate { get; set; }

        public string AuthorName { get; set; }

        public int AuthorId { get; set; }
    }
}