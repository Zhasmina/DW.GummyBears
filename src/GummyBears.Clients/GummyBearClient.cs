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

namespace GummyBears.Clients
{
    public class GummyBearClient
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

        public async Task<Response<UserProfile>> UpdateUserAsync(UserRequest request)
        {
            HttpRequestMessage httpRequestMessage = BuildRequestMessageWithBody(request, string.Format("users/{0}", request.Payload.Id), HttpMethod.Post);

            return await SendRequestAsync<UserProfile>(httpRequestMessage);
        }

        public async Task<Response<UserProfile>> GetUserAsync(UserProfileRequest request)
        {
            HttpRequestMessage httpRequestMessage = BuildRequestMessage(request, string.Format("users/{0}", request.UserId), HttpMethod.Get);

            return await SendRequestAsync<UserProfile>(httpRequestMessage);
        }

        public async Task<Response<Group>> GetAllUserGroups(UserProfileRequest request)
        {
            HttpRequestMessage httpRequestMessage = BuildRequestMessage(request, string.Format("users/{0}", request.UserId), HttpMethod.Get);

            return await SendRequestAsync<Group>(httpRequestMessage);
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
                throw new ConfigurationErrorsException("Missing setting: Bede.PlayerMessenger.Url");
            }

            var requestMessage = new HttpRequestMessage(httpMethod, string.Format("{0}/{1}{2}", new Uri(_gummyBearsUrl), location, querystring));
            requestMessage.Headers.Add("X-Correlation-Token", request.CorrelationToken);
            requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            if (requestMessage is AuthenticationTokenRequest)
            {
                requestMessage.Headers.Add("Authorization-Token", (request as AuthenticationTokenRequest).AuthenticationToken);
            }

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
