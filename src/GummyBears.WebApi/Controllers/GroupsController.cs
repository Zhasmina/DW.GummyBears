using GummyBears.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using GummyBears.Entities;

namespace GummyBears.WebApi.Controllers
{
    [RoutePrefix("groups")]
    public class GroupsController : BaseController
    {
        public GroupsController(IDbContext dbContext)
            : base(dbContext)
        { }

        [Route("groupId:int}")]
        [Authorize(Roles = "User")]
        public async Task<IEnumerable<GroupMessageEntity>> GetMessagesInGroup([FromUri]int groupId)
        {
            AuthenticationEntity authentication = await DbContext.AuthenticationRepo.GetSingleOrDefaultAsync(AuthenticationToken);

            GroupUserEntity userGroup = await DbContext.GroupsUsersRepo.GetByUserIdAndGroupId(authentication.UserId, groupId);

            if (userGroup == null)
            {
                ThrowHttpResponseException(System.Net.HttpStatusCode.Unauthorized, "Access is denied.");
            }

            GroupEntity group = await DbContext.GroupsRepo.GetSingleOrDefaultAsync(groupId);

            if (group == null)
            {
                ThrowHttpResponseException(System.Net.HttpStatusCode.NotFound, string.Format("Group with id {0} was not found.", groupId));
            }

            IEnumerable<GroupMessageEntity> messages = await DbContext.GroupMessagesRepo.GetGroupMessages(groupId);

            return messages;
        }
    }
}