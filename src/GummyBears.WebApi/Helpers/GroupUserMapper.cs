using GummyBears.Contracts;
using GummyBears.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GummyBears.WebApi.Helpers
{
    public static class GroupUserMapper
    {
        public static GroupUserEntity ToEntity(this GroupParticipants groupParticipants)
        {
            return new GroupUserEntity
            {
                GroupId = groupParticipants.GroupId,
                UserId = groupParticipants.ParticipantId,
                IsAdmin = false
            };
        }

        public static GroupParticipants ToContract(this GroupUserEntity groupUserEntity)
        {
            return new GroupParticipants
            {
                GroupId = groupUserEntity.GroupId,
                ParticipantId = groupUserEntity.UserId,
            };
        }
    }
}