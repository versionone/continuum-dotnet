using System.Collections.Generic;
using ContinuumDotNet.Api;
using ContinuumDotNet.Api.Flow;
using ContinuumDotNet.Data;
using ContinuumDotNet.Interfaces.Connection;
using ContinuumDotNet.Interfaces.Flow.Artifacts;
using ContinuumDotNet.Interfaces.Flow.Pipelines;
using RestSharp;

namespace ContinuumDotNet.Flow.Artifacts
{
    public class ArtifactManager : QueryBase<Artifact, List<Artifact>>, IArtifactManager
    {
        public ArtifactManager(IContinuumConnection continuumConnection)
        {
            Connection = continuumConnection;
             _restRequest = new RestRequest($"{Constants.API_PATH}/{Endpoints.GET_ARTIFACTS}", Method.GET);
        }

        public IArtifactManager WithName(string name)
        {
            _restRequest.AddQueryParameter(Parameters.ARTIFACT_NAME, name);
            return this;
        }
    }
}
