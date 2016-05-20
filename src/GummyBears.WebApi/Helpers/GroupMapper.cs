using GummyBears.Contracts;
using GummyBears.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GummyBears.WebApi.Helpers
{
    public static class GroupMapper
    {
        public static GroupEntity ToEntity(this Group group)
        {
            return new GroupEntity
            {
                Id = group.GroupId,
                Name = group.GroupName,
                AuthorId = group.AuthorId
            };
        }

        public static Group ToContract(this GroupEntity groupEntity)
        {
            return new Group
            {
                GroupId = groupEntity.Id,
                GroupName = groupEntity.Name,
                AuthorId = groupEntity.AuthorId
            };
        }
    }
}