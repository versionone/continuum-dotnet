using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace ContinuumDotNet.Interfaces.Connection
{
    public interface IContinuumConnection
    {
        IRestResponse MakeRequest(IRestRequest request);
    }
}
