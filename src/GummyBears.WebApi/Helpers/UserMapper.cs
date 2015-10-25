﻿using GummyBears.Contracts;
using GummyBears.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GummyBears.WebApi.Helpers
{
    public static class UserMapper
    {
        public static UserEntity ToEntity(this User user)
        {
            return new UserEntity
            {
                Country = user.Country,
                DateOfBirth = user.DateOfBirth,
                Description = user.Description,
                Email = user.Email,
                FirstName = user.FirstName,
                Id = user.Id,
                LastName = user.LastName,
                Password = user.Password,
                ProfilePicturePath = user.ProfilePicturePath,
                TelephoneNumber = user.TelephoneNumber,
                UserName = user.UserName
            };
        }

        public static User ToModel(this UserEntity user)
        {
            return new User
            {
                Country = user.Country,
                DateOfBirth = user.DateOfBirth,
                Description = user.Description,
                Email = user.Email,
                FirstName = user.FirstName,
                Id = user.Id,
                LastName = user.LastName,
                Password = user.Password,
                ProfilePicturePath = user.ProfilePicturePath,
                TelephoneNumber = user.TelephoneNumber,
                UserName = user.UserName
            };
        }
    }
}