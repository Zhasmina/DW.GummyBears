using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace GummyBears.WebApi.Helpers
{
    public static class RequestExtensions
    {
        public const string AuthorizationToken = "Authorization-Token";

        public static string GetAuthorizationToken(this HttpRequestMessage requestMessage)
        {
            return requestMessage.Headers.GetFirstOrNull(AuthorizationToken);
        }

        private static string GetFirstOrNull(this HttpRequestHeaders headers, string key)
        {
            IEnumerable<string> values;
            if (headers.TryGetValues(key, out values))
            {
                return values.FirstOrDefault();
            }

            return null;
        }
    }
}