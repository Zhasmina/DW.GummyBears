using GummyBears.Contracts;
using GummyBears.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GummyBears.WebApi.Helpers
{
    public static class GroupMessageMapper
    {
        public static GroupMessageEntity ToEntity(this GroupMessage groupMessage)
        {
            return new GroupMessageEntity
            {
                GroupId = groupMessage.GroupId,
                Message = groupMessage.Message,
                SendDate = groupMessage.SendDate
            };
        }

        public static GroupMessage ToContract(this GroupMessageEntity groupMessageEntity)
        {
            return new GroupMessage
            {
                GroupId = groupMessageEntity.GroupId,
                Message = groupMessageEntity.Message,
                SendDate = groupMessageEntity.SendDate
            };
        }
    }
}