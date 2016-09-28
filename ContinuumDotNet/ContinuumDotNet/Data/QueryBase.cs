using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ContinuumDotNet.Exceptions;
using ContinuumDotNet.Flow.Pipelines;
using ContinuumDotNet.Interfaces.Connection;
using ContinuumDotNet.Interfaces.Flow.Pipelines;
using Newtonsoft.Json;
using RestSharp;

namespace ContinuumDotNet.Data
{
    public class QueryBase<T, U>
    {
        public IContinuumConnection Connection { get; set; }

        internal RestRequest _restRequest;
        public T GetOne()
        {
            var exceptionHandler = new ExceptionHandler();
            var response = Connection.MakeRequest(_restRequest);

            var statusCodeId = (int)response.StatusCode;
            if (statusCodeId != 200)
            {
                switch ((int)response.StatusCode)
                {
                    case 280:
                        throw exceptionHandler.FindError(response.Content);

                    case 400:
                        dynamic errorContent = JsonConvert.DeserializeObject(response.Content);
                        throw exceptionHandler.FindError((string)errorContent.ErrorCode,
                            (string)errorContent.ErrorDetail);

                    case (int)HttpStatusCode.Unauthorized:
                        throw new UnauthorizedAccessException();

                    default:
                        throw new Exception();

                }
            }
            dynamic content = JsonConvert.DeserializeObject(response.Content);
            var json = JsonConvert.DeserializeObject<T>(content.Response.ToString());
            return json;
        }

        public U GetAll()
        {
            var exceptionHandler = new ExceptionHandler();
            var response = Connection.MakeRequest(_restRequest);

            var statusCodeId = (int)response.StatusCode;
            if (statusCodeId != 200)
            {
                switch ((int)response.StatusCode)
                {
                    case 280:
                        throw exceptionHandler.FindError(response.Content);

                    case 400:
                        dynamic errorContent = JsonConvert.DeserializeObject(response.Content);
                        throw exceptionHandler.FindError((string)errorContent.ErrorCode,
                            (string)errorContent.ErrorDetail);

                    case (int)HttpStatusCode.Unauthorized:
                        throw new UnauthorizedAccessException();

                    default:
                        throw new Exception();

                }
            }
            dynamic content = JsonConvert.DeserializeObject(response.Content);
            var json = JsonConvert.DeserializeObject<U>(content.Response.ToString());
            return json;
        }
    }
}
