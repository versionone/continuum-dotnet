using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContinuumDotNet.Utilities;

namespace ContinuumDotNet.Interfaces.Utilities
{
    public interface IRemotePsRunner
    {
        string ServerName { get; }
        PsResult RunScript(string script);
    }
}
