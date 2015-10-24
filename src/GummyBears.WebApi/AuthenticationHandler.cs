using GummyBears.Entities;
using GummyBears.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace GummyBears.WebApi
{
    public class TokenValidationHandler : DelegatingHandler
    {
        private IDbContext _dbContext;
        private TimeSpan _tokenLifespan;

        public TokenValidationHandler(IDbContext dbContext, TimeSpan tokenLifespan)
        {
            _dbContext = dbContext;
            _tokenLifespan = tokenLifespan;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {
                if (!request.Headers.Contains("Authorization-Token"))
                {
                    return await GenerateResponseMessage(HttpStatusCode.Unauthorized, "You need to include Authorization-Token header in your request");
                }

                var token = request.Headers.GetValues("Authorization-Token").FirstOrDefault();
                if (string.IsNullOrEmpty(token))
                {
                    return await GenerateResponseMessage(HttpStatusCode.Unauthorized, "Authorization-Token cannot be empty");

                }

                AuthenticationEntity authentication = await _dbContext..GetSingleOrDefaultAsync(token);
                if (authentication.LastSeen.Add(_tokenLifespan) >= DateTime.UtcNow)
                {
                    authentication.LastSeen = DateTime.UtcNow;
                    await _authenticationRepository.UpdateAsync(authentication);
                    UsersRepository
                }
                else
                {
                    return await GenerateResponseMessage(HttpStatusCode.Unauthorized, "Token expired");
                }
            }
            catch
            {
                return await GenerateResponseMessage(HttpStatusCode.InternalServerError, "Error encountered while attempting to process authorization token");
            }
            
            return await base.SendAsync(request, cancellationToken);
        }

        private async Task<HttpResponseMessage> GenerateResponseMessage(HttpStatusCode statusCode, string message)
        {
            TaskCompletionSource<HttpResponseMessage> tsc = new TaskCompletionSource<HttpResponseMessage>();
            tsc.SetResult(new HttpResponseMessage(statusCode)
            {
                Content = new StringContent(message)
            });

            return await tsc.Task;
        }
    }
}