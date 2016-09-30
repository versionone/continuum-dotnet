using System;
using System.Diagnostics;
using ContinuumDotNet.Exceptions.Connection;
using ContinuumDotNet.Interfaces.Connection;
using RestSharp;

namespace ContinuumDotNet.Connections
{
    public class ContinuumConnection : IContinuumConnection
    {
        private IRestClient _restClient { get; set; }

        public ContinuumConnection(string url, string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                throw new MissingTokenException();
            }

            if (string.IsNullOrEmpty(url) || !Uri.IsWellFormedUriString(url, UriKind.RelativeOrAbsolute))
            {
                throw new MissingOrInvalidUrlException();
            }
            _restClient = new RestClient() {BaseUrl = new Uri(url)};
            _restClient.AddDefaultHeader("Authorization", $"Token {token}");
            Trace.WriteLine($"Setting authorization token to {token}");
        }

        internal ContinuumConnection(IRestClient restClient, string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                throw new MissingTokenException();
            }
            _restClient = restClient;
            _restClient.AddDefaultHeader("Authorization", $"Token {token}");
            Trace.WriteLine($"Setting authorization token to {token}");
        }

        public IRestResponse MakeRequest(IRestRequest request)
        {
            if (_restClient.BaseUrl == null || _restClient.BaseUrl.ToString().Length == 0)
            {
                throw new MissingOrInvalidUrlException();
            }

            return _restClient.Execute(request);
        }
    }
}
