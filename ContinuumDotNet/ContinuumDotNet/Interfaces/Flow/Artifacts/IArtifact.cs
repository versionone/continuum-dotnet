using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContinuumDotNet.Interfaces.Common;

namespace ContinuumDotNet.Interfaces.Flow.Artifacts
{
    public interface IArtifact : IHasId, IHasName, IHasRevision, IHasVersion, IHasCreationDate
    {
        string Location { get; set; }
    }
}