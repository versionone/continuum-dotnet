using System.Collections.Generic;
using ContinuumDotNet.Flow.Artifacts;
using ContinuumDotNet.Flow.Pipelines;
using ContinuumDotNet.Interfaces.Connection;
using ContinuumDotNet.Interfaces.Data;

namespace ContinuumDotNet.Interfaces.Flow.Artifacts
{
    public interface IArtifactManager : IHasConnection, IHasQuery<Artifact, List<Artifact>>
    {
        IArtifactManager WithName(string name);
    }
}
