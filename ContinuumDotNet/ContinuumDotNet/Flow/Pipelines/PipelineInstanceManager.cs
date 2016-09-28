using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ContinuumDotNet.Api;
using ContinuumDotNet.Api.Flow;
using ContinuumDotNet.Data;
using ContinuumDotNet.Exceptions;
using ContinuumDotNet.Interfaces.Connection;
using ContinuumDotNet.Interfaces.Flow.Pipelines;
using Newtonsoft.Json;
using RestSharp;

namespace ContinuumDotNet.Flow.Pipelines
{
    public class PipelineInstanceManager : QueryBase<PipelineInstance>, IPipelineInstanceManager
    {
        public PipelineInstanceManager(IContinuumConnection continuumConnection)
        {
            Connection = continuumConnection;
             _restRequest = new RestRequest($"{Constants.API_PATH}/{Endpoints.GET_PIPELINE_INSTANCE}", Method.GET);
        }

        public IPipelineInstanceManager ById(string id)
        {
            _restRequest.AddQueryParameter(Parameters.PIPELINE_ID, id);
            return this;
        }
    }
}
