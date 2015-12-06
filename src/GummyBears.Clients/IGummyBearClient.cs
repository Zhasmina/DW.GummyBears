using System;
using System.Threading.Tasks;
namespace GummyBears.Clients
{
    public interface IGummyBearClient
    {
        Task<GummyBears.Clients.Responses.Response<GummyBears.Contracts.User>> CreateUserAsync(GummyBears.Clients.Requests.UserRequest request);
        Task<GummyBears.Clients.Responses.Response<GummyBears.Contracts.Group>> GetAllUserGroups(GummyBears.Clients.Requests.UserProfileRequest request);
        Task<GummyBears.Clients.Responses.Response<GummyBears.Contracts.UserProfile>> GetUserAsync(GummyBears.Clients.Requests.UserProfileRequest request);
        Task<GummyBears.Clients.Responses.Response<GummyBears.Contracts.AuthenticationData>> Login(GummyBears.Clients.Requests.AuthenticationRequest request);
        Task<GummyBears.Clients.Responses.Response<string>> Logout(GummyBears.Clients.Requests.AuthenticationTokenRequest request);
        Task<GummyBears.Clients.Responses.Response<GummyBears.Contracts.UserProfile>> UpdateUserAsync(GummyBears.Clients.Requests.AuthenticatedUserRequest request);
    }
}
