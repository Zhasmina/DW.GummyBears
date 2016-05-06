using System;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GummyBears.Clients.Responses;
using GummyBears.Contracts;
using GummyBears.Clients.Requests;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace GummyBears.Clients
{
    public class GummyBearClient : GummyBears.Clients.IGummyBearClient
    {
        private readonly HttpMessageInvoker _messageInvoker;
        private readonly string _gummyBearsUrl;
        public GummyBearClient(
            HttpMessageInvoker messageInvoker,
            IGummyBearClientSettings clientSettings)
        {
            _messageInvoker = messageInvoker;
            _gummyBearsUrl = clientSettings.BaseUrl;
        }

        public async Task<Response<User>> CreateUserAsync(UserRequest request)
        {
            HttpRequestMessage httpRequestMessage = BuildRequestMessageWithBody(request, "users", HttpMethod.Post);

            return await SendRequestAsync<User>( httpRequestMessage);
        }

        public async Task<Response<UserProfile>> UpdateUserAsync(AuthenticatedUserRequest request)
        {
            HttpRequestMessage httpRequestMessage = BuildRequestMessageWithBody(request, string.Format("users/{0}", request.Payload.Id), HttpMethod.Post);
            httpRequestMessage.Headers.Add("Authorization-Token", request.AuthenticationToken);

            return await SendRequestAsync<UserProfile>(httpRequestMessage);
        }

        public async Task<Response<UserProfile>> GetUserAsync(UserProfileRequest request)
        {
            HttpRequestMessage httpRequestMessage = BuildRequestMessage(request, string.Format("users/{0}", request.UserId), HttpMethod.Get);
            httpRequestMessage.Headers.Add("Authorization-Token", request.AuthenticationToken);
            
            return await SendRequestAsync<UserProfile>(httpRequestMessage);
        }

        public async Task<Response<List<Group>>> GetAllUserGroups(UserProfileRequest request)
        {
            HttpRequestMessage httpRequestMessage = BuildRequestMessage(request, string.Format("users/{0}/groups", request.UserId), HttpMethod.Get);
            httpRequestMessage.Headers.Add("Authorization-Token", request.AuthenticationToken);

            return await SendRequestAsync<List<Group>>(httpRequestMessage);
        }

        public async Task<Response<IEnumerable<Creation>>> GetUserCreations(UserProfileRequest request)
        {
            HttpRequestMessage httpRequestMessage = BuildRequestMessage(request, string.Format("users/{0}/creations", request.UserId), HttpMethod.Get);
            httpRequestMessage.Headers.Add("Authorization-Token", request.AuthenticationToken);

            return await SendRequestAsync<IEnumerable<Creation>>(httpRequestMessage);
        }

        public async Task<Response<Creation>> CreateUserCreations(AuthenticatedCreationRequest request)
        {
            HttpRequestMessage httpRequestMessage = BuildRequestMessageWithBody(request, string.Format("users/{0}/creations", request.Payload.UserId), HttpMethod.Post);
            httpRequestMessage.Headers.Add("Authorization-Token", request.AuthenticationToken);

            return await SendRequestAsync<Creation>(httpRequestMessage);
        }

        public async Task<Response<EmptyResponse>> DeleteCreation(AuthenticatedCreationRequest request)
        {
            HttpRequestMessage httpRequestMessage = BuildRequestMessage(request, string.Format("users/{0}/creations/{1}", request.Payload.UserId, request.Payload.CreationId), HttpMethod.Delete);
            httpRequestMessage.Headers.Add("Authorization-Token", request.AuthenticationToken);

            return await SendRequestAsync<EmptyResponse>(httpRequestMessage);
        }

        public async Task<Response<string>> Logout(AuthenticationTokenRequest request)
        {
            HttpRequestMessage message = BuildRequestMessage(request, "usercredentials", HttpMethod.Post);
            return await SendRequestAsync<string>(message);
        }

        public async Task<Response<AuthenticationData>> Login(AuthenticationRequest request)
        {
            HttpRequestMessage message = BuildRequestMessageWithBody(request, "usercredentials", HttpMethod.Post);
            return await SendRequestAsync<AuthenticationData>(message);
        }

        public async Task<Response<Feed>> PostToFeed(AuthenticatedFeedRequest request)
        {
            HttpRequestMessage httpRequestMessage = BuildRequestMessageWithBody(request, "api/feeds", HttpMethod.Post);
            httpRequestMessage.Headers.Add("Authorization-Token", request.AuthenticationToken);

            return await SendRequestAsync<Feed>(httpRequestMessage);
        }

        public async Task<Response<FeedsPage>> GetFeeds(PagedRequest request)
        {
            string requestString = "api/feeds" +
                (request.PageNumber.HasValue || request.PageSize.HasValue ? "?" : string.Empty) +
                (request.PageNumber.HasValue ? "pageNumber=" + request.PageNumber.Value.ToString() : string.Empty) +
                (request.PageSize.HasValue ? 
                    (request.PageNumber.HasValue ? 
                        "&pageSize=" + request.PageSize.Value.ToString() :
                        "pageSize=" + request.PageSize.Value.ToString()) :
                string.Empty); 
            HttpRequestMessage httpRequestMessage = BuildRequestMessage(request, requestString, HttpMethod.Get);
            httpRequestMessage.Headers.Add("Authorization-Token", request.AuthenticationToken);

            return await SendRequestAsync<FeedsPage>(httpRequestMessage);
        }

        public async Task<Response<Group>> CreateGroup(AuthenticatedGroupRequest request)
        {

            HttpRequestMessage httpRequestMessage = BuildRequestMessageWithBody(request,"groups", HttpMethod.Post);
            httpRequestMessage.Headers.Add("Authorization-Token", request.AuthenticationToken);

            return await SendRequestAsync<Group>(httpRequestMessage);
        }

        public async Task<Response<IEnumerable<GroupMessage>>> GetMessagesInGroup(GroupMessagesRequest request)
        {
            HttpRequestMessage httpRequestMessage = BuildRequestMessage(request, string.Format("groups/{0}?userId={1}", request.GroupId, request.UserId), HttpMethod.Get);
            httpRequestMessage.Headers.Add("Authorization-Token", request.AuthenticationToken);

            return await SendRequestAsync<IEnumerable<GroupMessage>>(httpRequestMessage);
        }

        public async Task<Response<IEnumerable<GroupParticipants>>> AddParticipantsInGroup(AuthenticatedGroupParticipantsRequest request)
        {

            HttpRequestMessage httpRequestMessage = BuildRequestMessageWithBody(request, string.Format("groups/{0}/participants", request.GroupId), HttpMethod.Post);
            httpRequestMessage.Headers.Add("Authorization-Token", request.AuthenticationToken);

            return await SendRequestAsync<IEnumerable<GroupParticipants>>(httpRequestMessage);
        }

        public async Task<Response<IEnumerable<GroupParticipants>>> GetParticipantsInGroup(AuthenticatedGroupRequest request)
        {
            HttpRequestMessage httpRequestMessage = BuildRequestMessage(request, string.Format("groups/{0}/participants", request.Payload.GroupId), HttpMethod.Get);
            httpRequestMessage.Headers.Add("Authorization-Token", request.AuthenticationToken);

            return await SendRequestAsync<IEnumerable<GroupParticipants>>(httpRequestMessage);
        }

        public async Task<Response<GroupCreation>> AttackFileToGroup(AuthenticatedGroupCreationsRequest request)
        {

            HttpRequestMessage httpRequestMessage = BuildRequestMessageWithBody(request, string.Format("groups/{0}/files", request.Payload.GroupId), HttpMethod.Post);
            httpRequestMessage.Headers.Add("Authorization-Token", request.AuthenticationToken);

            return await SendRequestAsync<GroupCreation>(httpRequestMessage);
        }

        public async Task<Response<IEnumerable<Creation>>> GetAttachedFilesInGroup(AuthenticatedGroupCreationsRequest request)
        {
            HttpRequestMessage httpRequestMessage = BuildRequestMessage(request, string.Format("groups/{0}/files", request.Payload.GroupId), HttpMethod.Get);
            httpRequestMessage.Headers.Add("Authorization-Token", request.AuthenticationToken);

            return await SendRequestAsync<IEnumerable<Creation>>(httpRequestMessage);
        }

        private async Task<Response<TResponse>> SendRequestAsync<TResponse>(HttpRequestMessage requestMessage)
        {
            var response = await _messageInvoker.SendAsync(requestMessage, CancellationToken.None).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                return new Response<TResponse>
                {
                    Status = Status.Success,
                    Payload = response.Content == null ? default(TResponse) : await response.Content.ReadAsAsync<TResponse>().ConfigureAwait(false)
                };
            }

            var errors = await response.Content.ReadAsAsync<Error[]>().ConfigureAwait(false);

            return new Response<TResponse>
            {
                Status = Status.Failed,
                Errors = errors.Select(error => error.Message).ToList()
            };
        }

        private async Task<Response> SendRequestAsync(HttpRequestMessage requestMessage, Action logRequest, Action logResponse)
        {
            logRequest();

            var response = await _messageInvoker.SendAsync(requestMessage, CancellationToken.None).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                logResponse();

                return new Response
                {
                    Status = Status.Success,
                };
            }

            var errors = await response.Content.ReadAsAsync<Error[]>().ConfigureAwait(false);

            return new Response
            {
                Status = Status.Failed,
                Errors = errors.Select(error => error.Message).ToList()
            };
        }

        private HttpRequestMessage BuildRequestMessage(IRequest request, string location, HttpMethod httpMethod, string querystring = "")
        {
            if (string.IsNullOrEmpty(_gummyBearsUrl))
            {
                throw new ConfigurationErrorsException("Missing setting: GummyBears.WebApi.Url");
            }

            var requestMessage = new HttpRequestMessage(httpMethod, string.Format("{0}/{1}{2}", new Uri(_gummyBearsUrl), location, querystring));
            requestMessage.Headers.Add("X-Correlation-Token", request.CorrelationToken);

            return requestMessage;
        }

        private HttpRequestMessage BuildRequestMessageWithBody<T>(RequestBase<T> request, string location, HttpMethod httpMethod, string querystring = "")
            where T : new()
        {
            var requestMessage = BuildRequestMessage(request, location, httpMethod, querystring);
            requestMessage.Content = SerialiseRequestAsJson(request);
            return requestMessage;
        }

        private StringContent SerialiseRequestAsJson<T>(RequestBase<T> request) where T : new()
        {
            return new StringContent(JsonConvert.SerializeObject(request.Payload), Encoding.UTF8, "application/json");
        }

    }
}
