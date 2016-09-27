using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContinuumDotNet.Connection;
using ContinuumDotNet.Interfaces.Connection;

namespace ContinuumDotNet.Interfaces.Flow.Pipelines
{
    public interface IPipelineInstance : IHasConnection
    {
        string Name { get; set; }
    }
}
