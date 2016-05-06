﻿using System;
using System.ComponentModel.DataAnnotations;

namespace GummyBears.Contracts
{
    public class UserProfile
    {
        public int Id { get; set; }

        [RegularExpression(@"^[A-Za-z][A-Za-z0-9._]{4,15}$",
            ErrorMessage = "Username can contain only letters, digits, dots and down slashes. Length of the name must be between 4 and 15 characters")]
        public string UserName { get; set; }

        [StringLength(20, MinimumLength = 1, ErrorMessage = "First name should be between 1 and 20 characters long")]
        public string FirstName { get; set; }

        [StringLength(20, MinimumLength = 1, ErrorMessage = "Last name should be between 1 and 20 characters long")]
        public string LastName { get; set; }

        [EmailAddress(ErrorMessage = "Incorrect email format")]
        public string Email { get; set; }

        public string TelephoneNumber { get; set; }
        
        public DateTime DateOfBirth { get; set; }

        public string Description { get; set; }

        public string Country { get; set; }

        public string ProfilePicturePath { get; set; }
    }
}
