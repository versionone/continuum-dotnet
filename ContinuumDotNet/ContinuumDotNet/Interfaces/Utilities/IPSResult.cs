using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContinuumDotNet.Interfaces.Utilities
{
    public interface IPsResult
    {
        string StdOut { get; set; }
        string StdErr { get; set; }
    }
}
