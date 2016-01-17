using GummyBears.Clients.Requests;
using GummyBears.Clients.Responses;
using GummyBears.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace GummyBears.Clients
{
    public interface IGummyBearClient
    {
        Task<GummyBears.Clients.Responses.Response<GummyBears.Contracts.User>> CreateUserAsync(GummyBears.Clients.Requests.UserRequest request);
        Task<Response<List<Group>>> GetAllUserGroups(UserProfileRequest request);
        Task<Response<UserProfile>> GetUserAsync(GummyBears.Clients.Requests.UserProfileRequest request);
        Task<Response<AuthenticationData>> Login(GummyBears.Clients.Requests.AuthenticationRequest request);
        Task<Response<string>> Logout(AuthenticationTokenRequest request);
        Task<Response<UserProfile>> UpdateUserAsync(AuthenticatedUserRequest request);
        Task<Response<IEnumerable<Creation>>> GetUserCreations(UserProfileRequest request);
        Task<Response<Creation>> CreateUserCreations(AuthenticatedCreationRequest request);
        Task<Response<EmptyResponse>> DeleteCreation(AuthenticatedCreationRequest request);

        Task<Response<FeedsPage>> GetFeeds(PagedRequest request);

        Task<Response<Feed>> PostToFeed(AuthenticatedFeedRequest request);

        Task<Response<Group>> CreateGroup(AuthenticatedGroupRequest request);

        Task<Response<IEnumerable<GroupMessage>>> GetMessagesInGroup(GroupMessagesRequest request);

        Task<Response<IEnumerable<GroupParticipants>>> AddParticipantsInGroup(AuthenticatedGroupParticipantsRequest request);

        Task<Response<IEnumerable<GroupParticipants>>> GetParticipantsInGroup(AuthenticatedGroupRequest request);

        Task<Response<GroupCreation>> AttackFileToGroup(AuthenticatedGroupCreationsRequest request);

        Task<Response<IEnumerable<Creation>>> GetAttachedFilesInGroup(AuthenticatedGroupCreationsRequest request);
    }
}
