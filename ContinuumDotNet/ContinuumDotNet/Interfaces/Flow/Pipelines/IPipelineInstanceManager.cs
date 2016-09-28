using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContinuumDotNet.Flow.Pipelines;
using ContinuumDotNet.Interfaces.Connection;
using ContinuumDotNet.Interfaces.Data;

namespace ContinuumDotNet.Interfaces.Flow.Pipelines
{
    public interface IPipelineInstanceManager : IHasConnection, IHasQuery<PipelineInstance, List<PipelineInstance>>
    {
        IPipelineInstanceManager ById(string id);
    }
}
