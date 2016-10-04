using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContinuumDotNet.Interfaces.Utilities;

namespace ContinuumDotNet.Utilities
{
    public class PsResult : IPsResult
    {
        public string StdOut { get; set; }
        public string StdErr { get; set; }
    }
}
