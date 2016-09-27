using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContinuumDotNet.Interfaces.Connection;

namespace ContinuumDotNet.Interfaces.Flow.Pipelines
{
    public interface IPipelineInstanceManager : IHasConnection
    {
        IPipelineInstance Get(string id);
    }
}
