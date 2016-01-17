using GummyBears.Contracts;
using GummyBears.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GummyBears.WebApi.Helpers
{
    public static class GroupCreationMapper
    {
        public static GroupCreation ToContract(this GroupCreationEntity groupCreationEntity)
        {
            return new GroupCreation
            {
                Id = groupCreationEntity.Id,
                CreationId = groupCreationEntity.CreationId,
                GroupId = groupCreationEntity.GroupId
            };
        }
    }
}