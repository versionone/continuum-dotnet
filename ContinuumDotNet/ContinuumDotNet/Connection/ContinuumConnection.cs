using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using ContinuumDotNet.Exceptions.Connection;
using ContinuumDotNet.Interfaces.Connection;
using RestSharp;

namespace ContinuumDotNet.Connection
{
    public class ContinuumConnection : IContinuumConnection
    {
        private IRestClient _restClient { get; set; }

        public ContinuumConnection(IRestClient restClient, string token)
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
                throw new MissingUrlException();
            }

            return _restClient.Execute(request);
        }
    }
}
