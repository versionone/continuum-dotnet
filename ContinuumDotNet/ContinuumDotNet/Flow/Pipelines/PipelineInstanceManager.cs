using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ContinuumDotNet.Api;
using ContinuumDotNet.Api.Flow;
using ContinuumDotNet.Exceptions;
using ContinuumDotNet.Interfaces.Connection;
using ContinuumDotNet.Interfaces.Flow.Pipelines;
using Newtonsoft.Json;
using RestSharp;

namespace ContinuumDotNet.Flow.Pipelines
{
    public class PipelineInstanceManager : IPipelineInstanceManager
    {
        public IContinuumConnection Connection { get; set; }
        public PipelineInstanceManager(IContinuumConnection continuumConnection)
        {
            Connection = continuumConnection;
        }

        public IPipelineInstance Get(string id)
        {
            var restRequest = new RestRequest($"{Constants.API_PATH}/{Endpoints.GET_PIPELINE_INSTANCE}", Method.GET);
                    var exceptionHandler = new ExceptionHandler();
            restRequest.AddQueryParameter(Parameters.PIPELINE_ID, id);
            var response = Connection.MakeRequest(restRequest);

            var statusCodeId = (int) response.StatusCode;
            if (statusCodeId != 200)
            {
                switch ((int) response.StatusCode)
                {
                    case 280:
                        throw exceptionHandler.FindError(response.Content);

                    case 400:
                        dynamic errorContent = JsonConvert.DeserializeObject(response.Content);
                        throw exceptionHandler.FindError((string) errorContent.ErrorCode,
                            (string) errorContent.ErrorDetail);

                    case (int) HttpStatusCode.Unauthorized:
                        throw new UnauthorizedAccessException();

                    default:
                        throw new Exception();

                }
            }
            dynamic content = JsonConvert.DeserializeObject(response.Content);
            var pipelineInstance = JsonConvert.DeserializeObject<PipelineInstance>(content.Response.ToString());
            return pipelineInstance;
        }

    }
}
